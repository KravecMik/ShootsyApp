using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Files")]
    public class FilesController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        InternalConstants _internalConstants;

        public FilesController(IFileRepository fileRepository, IUserRepository userRepository, InternalConstants internalConstants)
        {
            _fileRepository = fileRepository;
            _internalConstants = internalConstants;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFileAsync(CreateFileModel model, CancellationToken cancellationToken = default)
        {
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
                ContentPath = fullPath,
                isDeleted = false,

            };

            var id = await _fileRepository.CreateAsync(file, cancellationToken);
            return StatusCode(201, id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileByIdAsync([FromRoute] [Required(ErrorMessage = "Укажите идентификатор пользователя")] int id, CancellationToken cancellationToken = default)
        {
            var file = await _fileRepository.GetByIdAsync(id, cancellationToken);
            if (file is null)
                return NotFound();

            return Ok(file);
        }


        //[HttpDelete]
        //public async Task<IActionResult> DeleteFileAsync(int id, CancellationToken cancellationToken = default)
        //{

        //}
    }
}
