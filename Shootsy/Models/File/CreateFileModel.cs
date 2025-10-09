using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.File
{
    public class CreateFileModel : BaseFilesModel
    {
        [Required(ErrorMessage = "Укажите идентификатор пользователя")]
        [FromForm]
        public string User { get; set; }

        [Required(ErrorMessage = "Укажите файл пользователя")]
        public IFormFile File { get; set; }

        public string Extension { get; set; }
    }
}
