using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Models.File;
using Shootsy.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Files")]
    public class FilesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        InternalConstants _internalConstants;
        private readonly HttpClient _httpClient;

        public FilesController(IFileRepository fileRepository, IUserRepository userRepository, InternalConstants internalConstants, IMapper mapper, HttpClient httpClient)
        {
            _fileRepository = fileRepository;
            _internalConstants = internalConstants;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFileAsync(CreateFileModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var user = await _userRepository.GetByIdAsync(Convert.ToInt16(model.User), cancellationToken);
            if (user is null)
                return NotFound();

            var isForbidden = await _httpClient.GetAsync($"{_internalConstants.BaseUrl}/users/session/{model.Session}/access-to/{model.User}");
            if (!isForbidden.IsSuccessStatusCode)
                return StatusCode(403);

            //var dir = _internalConstants.BaseFilePath + @$"/{model.User}/";

            //if (!Directory.Exists(dir))
            //    Directory.CreateDirectory(dir);

            //var fullPath = dir + model.File.FileName;
            //var ext = Path.GetExtension(fullPath);

            if (!_internalConstants.SupportedFileExtensions.Contains(model.Extension.ToLower()))
                return BadRequest();

            //using (var fileStream = new FileStream(fullPath, FileMode.Create))
            //{
            //    await model.File.CopyToAsync(fileStream);
            //}

            var file = new FileDto
            {
                User = Convert.ToInt16(model.User),
                FileName = model.File.FileName,
                Extension = model.Extension,
                ContentPath = _internalConstants.BaseFilePath + model.File.FileName + model.Extension
            };

            var id = await _fileRepository.CreateAsync(file, cancellationToken);
            return StatusCode(201, id);
        }


        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetFileByIdAsync", Summary = "Получение файла по идентификатору")]
        [SwaggerRequestExample(typeof(GetFileByIdModel), typeof(GetFileByIdRequestExample))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(FileModelResponse))]
        [SwaggerResponseExample(200, typeof(FileModelResponseExample))]
        public async Task<IActionResult> GetFileByIdAsync([FromQuery] GetFileByIdModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var file = await _fileRepository.GetByIdAsync(model.Id, cancellationToken);
            if (file is null)
                return NotFound();

            var result = _mapper.Map<FileModelResponse>(file);
            return Ok(result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Получить список файлов")]
        [SwaggerRequestExample(typeof(GetFilesModel), typeof(GetFilesModelRequestExample))] 
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(IEnumerable<FileModelResponse>))]
        [SwaggerResponseExample(200, typeof(GetFilesResponseExample))]
        public async Task<IActionResult> GetFilesAsync([FromQuery]GetFilesModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var files = await _fileRepository.GetListAsync(Convert.ToInt16(model.Limit), model.Offset, model.Filter, model.Sort, cancellationToken);
            var result = _mapper.Map<IEnumerable<FileModelResponse>>(files);
            return Ok(result.OrderByDescending(x => x.Id));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateFileAsync(
        UpdateFileModel model,
        CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var currentFile = await _fileRepository.GetByIdAsync(model.Id, cancellationToken);
            if (currentFile is null)
                return NotFound();

            var isForbidden = await _httpClient.GetAsync($"{_internalConstants.BaseUrl}/users/session/{model.Session}/access-to/{model.Id}");
            if (!isForbidden.IsSuccessStatusCode)
                return StatusCode(403);

            await _fileRepository.UpdateAsync(currentFile, model.PatchDocument, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFileByIdAsync(GetFileByIdModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var file = await _fileRepository.GetByIdAsync(model.Id, cancellationToken);
            if (file is null)
                return NotFound();

            var isForbidden = await _httpClient.GetAsync($"{_internalConstants.BaseUrl}/users/session/{model.Session}/access-to/{model.Id}");
            if (!isForbidden.IsSuccessStatusCode)
                return StatusCode(403);

            await _fileRepository.DeleteByIdAsync(model.Id, cancellationToken);

            if (System.IO.File.Exists(file.ContentPath))
                System.IO.File.Delete(file.ContentPath);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteManyFilesAsync(DeleteManyFilesModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();
            foreach (var id in model.Ids)
            {
                var file = await _fileRepository.GetByIdAsync(id, cancellationToken);
                if (file is null)
                    return NotFound();

                var isForbidden = await _httpClient.GetAsync($"{_internalConstants.BaseUrl}/users/session/{model.Session}/access-to/{id}");
                if (!isForbidden.IsSuccessStatusCode)
                    return StatusCode(403);

                await _fileRepository.DeleteByIdAsync(id, cancellationToken);

                if (System.IO.File.Exists(file.ContentPath))
                    System.IO.File.Delete(file.ContentPath);
            }
            return NoContent();
        }
    }
}
