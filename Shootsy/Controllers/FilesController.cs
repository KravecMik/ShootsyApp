using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Models.File;
using Shootsy.Repositories;

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

        public FilesController(IFileRepository fileRepository, IUserRepository userRepository, InternalConstants internalConstants, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _internalConstants = internalConstants;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFileAsync(CreateFileModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.isAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var user = await _userRepository.GetByIdAsync(Convert.ToInt16(model.User), cancellationToken);
            if (user is null)
                return NotFound();

            var dir = _internalConstants.BaseFilePath + @$"/{model.User}/";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var fullPath = dir + model.File.FileName;
            var ext = Path.GetExtension(fullPath);

            if (!_internalConstants.SupportedFileExtensions.Contains(ext.ToLower()))
                return BadRequest();

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }

            var file = new FileDto
            {
                User = Convert.ToInt16(model.User),
                FileName = model.File.FileName,
                Extension = ext,
                ContentPath = fullPath
            };

            var id = await _fileRepository.CreateAsync(file, cancellationToken);
            return StatusCode(201, id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileByIdAsync(GetFileByIdModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.isAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var file = await _fileRepository.GetByIdAsync(model.Id, cancellationToken);
            if (file is null)
                return NotFound();

            var result = _mapper.Map<FileModelResponse>(file);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilesAsync(GetFilesModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.isAuthorized(model.Session, cancellationToken);
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
            var isSessionValid = await _userRepository.isAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var currentFile = await _fileRepository.GetByIdAsync(model.Id, cancellationToken);
            if (currentFile is null)
                return NotFound();

            await _fileRepository.UpdateAsync(currentFile, model.PatchDocument, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFileByIdAsync(GetFileByIdModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.isAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            var file = await _fileRepository.GetByIdAsync(model.Id, cancellationToken);
            if (file is null)
                return NotFound();

            await _fileRepository.DeleteByIdAsync(model.Id, cancellationToken);

            if (System.IO.File.Exists(file.ContentPath))
                System.IO.File.Delete(file.ContentPath);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteManyFilesAsync(DeleteManyFilesModel model, CancellationToken cancellationToken = default)
        {
            var isSessionValid = await _userRepository.isAuthorized(model.Session, cancellationToken);
            if (!isSessionValid)
                return Unauthorized();

            foreach (var id in model.Ids)
            {
                var file = await _fileRepository.GetByIdAsync(id, cancellationToken);
                if (file is null)
                    return NotFound();
                await _fileRepository.DeleteByIdAsync(id, cancellationToken);

                if (System.IO.File.Exists(file.ContentPath))
                    System.IO.File.Delete(file.ContentPath);
            }
            return NoContent();
        }
    }
}
