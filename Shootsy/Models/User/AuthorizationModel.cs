using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.User
{
    public class AuthorizationModel
    {
        [Required(ErrorMessage = "Укажите имя пользователя")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Укажите пароль пользователя")]
        public string Password { get; set; }
    }
}
