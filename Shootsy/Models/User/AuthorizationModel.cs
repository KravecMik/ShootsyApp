using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.User
{
    public class AuthorizationModel
    {
        [Required(ErrorMessage = "Укажите логин пользователя")]
        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль пользователя")]
        public string Password { get; set; }
    }
}
