using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.User
{
    public class SignInRequestModel
    {
        [Required(ErrorMessage = "Укажите логин пользователя")]
        public required string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        public required string Password { get; set; }
    }
}
