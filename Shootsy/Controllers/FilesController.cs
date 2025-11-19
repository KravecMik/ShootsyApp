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

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Files")]
    public class FilesController : BaseController
    {
        private readonly IObjectStorage _objectStorage;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly InternalConstants _internalConstants;

        public FilesController(IFileRepository fileRepository, IUserRepository userRepository, InternalConstants internalConstants, IMapper mapper, IObjectStorage objectStorage)
        {
            _fileRepository = fileRepository;
            _internalConstants = internalConstants;
            _userRepository = userRepository;
            _mapper = mapper;
            _objectStorage = objectStorage;
        }

        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Создание карточки файла")]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(String))]
        public async Task<IActionResult> CreateFileAsync([FromForm] CreateFileRequestModel request, CancellationToken cancellationToken = default)
        {
            var userId = await GetCurrentUserIdAsync(_userRepository, cancellationToken);

            if (!_internalConstants.SupportedFileExtensions.Contains(Path.GetExtension(request.File.FileName).ToLower()))
            {
                ModelState.AddModelError("File.Extension", "Файл с таким расширением не поддерживается");
                return ValidationProblem();
            }

            var objectKey = $"{userId.Id}/{Guid.NewGuid():N}/{Path.GetFileName(request.File.FileName)}";
            await using var stream = request.File.OpenReadStream();
            var (key, publicUrl) = await _objectStorage.UploadAsync(stream, objectKey, request.File.ContentType ?? "", cancellationToken);

            var fileId = await _fileRepository.CreateFileAsync(new FileStorageEntity
            {
                UserId = userId.Id,
                FileInfo = new Database.Mongo.FileInfo
                {
                    FileName = request.File.FileName,
                    Extension = Path.GetExtension(request.File.FileName),
                    ObjectKey = key,
                    ContentPath = publicUrl
                }
            }, cancellationToken);

            return CreatedAtAction(nameof(CreateFileAsync), fileId);
        }

        [Authorize]
        [HttpGet("{fileId}")]
        [SwaggerOperation(Summary = "Получение карточки файла по идентификатору")]
        [SwaggerResponse(statusCode: 200, description: "OK")]
        [SwaggerResponseExample(200, typeof(GetFileByIdResponseExampleModel))]
        public async Task<IActionResult> GetFileByIdAsync([FromRoute] string fileId, CancellationToken cancellationToken = default)
        {
            var file = await _fileRepository.GetFileByIdAsync(fileId, cancellationToken);
            if (file is null)
                return NotFound();
            return Ok(file);
        }

        [Authorize]
        [HttpPatch("{fileId}")]
        [Consumes("application/json-patch+json")]
        [SwaggerOperation(Summary = "Обновить карточку файла")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdateFileAsync([FromRoute] string fileId, [FromBody] JsonPatchDocument<FileStorageEntity> patch, CancellationToken cancellationToken = default)
        {

            var entity = await _fileRepository.GetFileByIdAsync(fileId, cancellationToken);
            if (entity is null)
                return NotFound();

            var (hasAccess, errorResult) = await CheckAccessByUserIdAsync(entity.UserId, _userRepository, cancellationToken);
            if (!hasAccess)
            {
                return errorResult!;
            }

            var targetPath = patch.Operations.Select(x => x).ToList();

            foreach (var item in targetPath)
            {
                if (item.path.ToLower().Contains("userid"))
                {
                    var user = await _userRepository.GetUserByIdAsync(int.Parse(item.value.ToString()), cancellationToken);
                    if (user is null)
                        return NotFound();
                }
                if (item.path.ToLower().Contains("extension"))
                {
                    if (!_internalConstants.SupportedFileExtensions.Contains(item.value.ToString().ToLower()))
                    {
                        ModelState.AddModelError("Extension", "Указанный тип файла не поддерживается");
                        return ValidationProblem();
                    }
                }
                if (item.path.ToLower().Contains("contentpath"))
                {
                    ModelState.AddModelError("ContentPath", "Данное поле редактировать запрещено");
                    return ValidationProblem();
                }
                if (item.path.ToLower().Contains("objectkey"))
                {
                    ModelState.AddModelError("ObjectKey", "Данное поле редактировать запрещено");
                    return ValidationProblem();
                }
            }

            patch.ApplyTo(entity, ModelState);

            entity.EditDate = DateTime.UtcNow;

            var updated = await _fileRepository.UpdateFileAsync(entity, cancellationToken);
            if (!updated)
                return StatusCode(500, "Не удалось обновить карточку файла");

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{fileId}")]
        [SwaggerOperation(Summary = "Удалить файл по идентификатору")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteFileByIdAsync([FromRoute] string fileId, CancellationToken cancellationToken = default)
        {
            var file = await _fileRepository.GetFileByIdAsync(fileId, cancellationToken);
            if (file is null)
                return NotFound();

            var (hasAccess, errorResult) = await CheckAccessByUserIdAsync(file.UserId, _userRepository, cancellationToken);
            if (!hasAccess)
            {
                return errorResult!;
            }

            await _objectStorage.DeleteAsync(file.FileInfo.ObjectKey, cancellationToken);
            await _fileRepository.DeleteFileByIdAsync(fileId, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpGet("linked-user/{userId:int}")]
        [SwaggerOperation(Summary = "Получить список файлов пользователя")]
        [SwaggerResponse(statusCode: 200, description: "OK")]
        [SwaggerResponseExample(200, typeof(GetFileListResponseExampleModel))]
        public async Task<IActionResult> GetFilesListByUserIdAsync([FromRoute] int userId, CancellationToken cancellationToken = default)
        {
            var result = await _fileRepository.GetFilesListByUserIdAsync(userId, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("linked-user/{userId:int}")]
        [SwaggerOperation(Summary = "Удалить все файлы по идентификатору пользователя")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteUserFilesAsync([FromRoute] int userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
                return NotFound();

            var (hasAccess, errorResult) = await CheckAccessByUserIdAsync(user.Id, _userRepository, cancellationToken);
            if (!hasAccess)
            {
                return errorResult!;
            }

            var userFilesList = await _fileRepository.GetFilesListByUserIdAsync(userId, cancellationToken);

            foreach (var file in userFilesList)
            {
                await _objectStorage.DeleteAsync(file.FileInfo.ObjectKey, cancellationToken);
                await _fileRepository.DeleteFileByIdAsync(file.Id, cancellationToken);
            }
            return NoContent();
        }
    }
}