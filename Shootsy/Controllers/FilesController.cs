using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Database.Mongo;
using Shootsy.Models.File;
using Shootsy.Models.File.Swagger;
using Shootsy.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.IO;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Files")]
    public class FilesController : ControllerBase
    {
        private readonly IObjectStorage _objectStorage;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly InternalConstants _internalConstants;
        private readonly HttpClient _httpClient;

        public FilesController(IFileRepository fileRepository, IUserRepository userRepository, InternalConstants internalConstants, IMapper mapper, HttpClient httpClient, IObjectStorage objectStorage)
        {
            _fileRepository = fileRepository;
            _internalConstants = internalConstants;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _objectStorage = objectStorage;
        }

        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Создание карточки файла")]
        [SwaggerResponse(statusCode: 201, description: "OK", type: typeof(CreateFileResponse))]
        [SwaggerResponseExample(201, typeof(CreateFileResponseExample))]
        public async Task<IActionResult> CreateFileAsync([FromForm] CreateFileRequest model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (user is null) return NotFound();

            if (!_internalConstants.SupportedFileExtensions.Contains(Path.GetExtension(model.File.FileName).ToLower()))
            {
                ModelState.AddModelError("File.Extension", "Файл с таким расширением не поддерживается");
                return ValidationProblem();
            }

            var objectKey = $"{model.IdUser}/{Guid.NewGuid():N}/{Path.GetFileName(model.File.FileName)}";
            await using var stream = model.File.OpenReadStream();
            var (key, publicUrl) = await _objectStorage.UploadAsync(stream, objectKey, model.File.ContentType ?? "", cancellationToken);

            var fileId = await _fileRepository.CreateAsync(new FileStorageEntity
            {
                IdUser = model.IdUser,
                FileInfo = new Database.Mongo.FileInfo
                {
                    FileName = model.File.FileName,
                    Extension = Path.GetExtension(model.File.FileName),
                    ObjectKey = key,
                    ContentPath = publicUrl
                }
            }, cancellationToken);

            return CreatedAtAction(nameof(GetFileByIdAsync), new { id = fileId }, new { id = fileId, objectKey = key, url = publicUrl });
        }

        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получение карточки файла по идентификатору")]
        [SwaggerRequestExample(typeof(GetFileByIdRequest), typeof(GetFileByIdRequestExample))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(GetFileByIdResponse))]
        [SwaggerResponseExample(200, typeof(GetFileByIdResponseExample))]
        public async Task<IActionResult> GetFileByIdAsync([FromQuery] GetFileByIdRequest model, CancellationToken cancellationToken = default)
        {
            var file = await _fileRepository.GetByIdAsync(model.IdFile, cancellationToken);
            if (file is null) return NotFound();
            return Ok(file);
        }

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список карточек файлов")]
        [SwaggerRequestExample(typeof(FileStorageFilter), typeof(GetFileListRequestExample))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(IEnumerable<GetFileByIdResponse>))]
        [SwaggerResponseExample(200, typeof(GetFileListResponseExample))]
        public async Task<IActionResult> GetFilesAsync([FromQuery] FileStorageFilter filter, CancellationToken cancellationToken = default)
        {
            var result = await _fileRepository.GetListAsync(filter, cancellationToken);
            return Ok(result.Item1);
        }

        [Authorize]
        [HttpPatch("{id}")]
        [Consumes("application/json-patch+json")]
        [SwaggerOperation(Summary = "Обновить карточку файла")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdateFileAsync([FromRoute(Name = "id")] string id, [FromBody] JsonPatchDocument<FileStorageEntity> patch, CancellationToken cancellationToken = default)
        {
            var entity = await _fileRepository.GetByIdAsync(id, cancellationToken);
            if (entity is null) return NotFound();

            var targetPath = patch.Operations.Select(x => x).ToList();

            foreach (var item in targetPath)
            {
                if (item.path.ToLower().Contains("iduser"))
                {
                    var user = await _userRepository.GetByIdAsync(int.Parse(item.value.ToString()), cancellationToken);
                    if (user is null) return NotFound();
                }
                if (item.path.ToLower().Contains("extension"))
                {
                    if (!_internalConstants.SupportedFileExtensions.Contains(item.value.ToString().ToLower()))
                        ModelState.AddModelError("Extension", "Указанный тип файла не поддерживается");
                    return ValidationProblem();
                }
                if (item.path.ToLower().Contains("ContentPath"))
                {
                    ModelState.AddModelError("ContentPath", "Данное поле редактировать запрещено");
                    return ValidationProblem();
                }
                if (item.path.ToLower().Contains("ObjectKey"))
                {
                    ModelState.AddModelError("ObjectKey", "Данное поле редактировать запрещено");
                    return ValidationProblem();
                }
            }

            patch.ApplyTo(entity, ModelState);
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            entity.EditDate = DateTime.UtcNow;

            var updated = await _fileRepository.ReplaceAsync(entity, cancellationToken);
            if (!updated) return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить файл по идентификатору")]
        [SwaggerRequestExample(typeof(GetFileByIdRequest), typeof(GetFileByIdRequestExample))]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteFileByIdAsync([FromQuery] GetFileByIdRequest model, CancellationToken cancellationToken = default)
        {
            var file = await _fileRepository.GetByIdAsync(model.IdFile, cancellationToken);
            if (file is null) return NotFound();

            await _objectStorage.DeleteAsync(file.FileInfo.ObjectKey, cancellationToken);
            await _fileRepository.DeleteByIdAsync(model.IdFile, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("iduser={id}")]
        [SwaggerOperation(Summary = "Удалить все файлы по идентификатору пользователя")]
        [SwaggerRequestExample(typeof(DeleteUserFilesRequest), typeof(DeleteUserFilesRequestExample))]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteUserFilesAsync([FromQuery] DeleteUserFilesRequest model, CancellationToken cancellationToken = default)
        {
            var fileList = await _fileRepository.GetListAsync(new FileStorageFilter { UserId = model.IdUser }, cancellationToken);

            foreach (var file in fileList.Item1)
            {
                await _objectStorage.DeleteAsync(file.FileInfo.ObjectKey, cancellationToken);
                await _fileRepository.DeleteByIdAsync(file.Id, cancellationToken);
            }
            return NoContent();
        }
    }
}
