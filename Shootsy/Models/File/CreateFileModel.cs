using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.File
{
    public class CreateFileModel
    {
        [Required(ErrorMessage = "Укажите идентификатор пользователя")]
        [FromForm]
        public string User { get; set; }

        [Required(ErrorMessage = "Укажите файл пользователя")]
        public IFormFile File { get; set; }

        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
