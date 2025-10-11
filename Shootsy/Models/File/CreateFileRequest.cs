using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.File
{
    public class CreateFileRequest
    {
        [Required(ErrorMessage = "Укажите идентификатор пользователя")]
        [FromForm]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Прикрепите файл")]
        public required IFormFile File { get; set; }
    }
}
