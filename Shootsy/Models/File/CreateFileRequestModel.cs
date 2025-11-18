using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.File
{
    public class CreateFileRequestModel
    {
        [Required(ErrorMessage = "Прикрепите файл")]
        public required IFormFile File { get; set; }
    }
}