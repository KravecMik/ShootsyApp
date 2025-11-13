using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.User
{
    public class SignUpRequestModel
    {
        [Required(ErrorMessage = "Укажите логин пользователя")]
        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public required string Login { get; init; }

        [Required(ErrorMessage = "Укажите пароль пользователя")]
        [MinLength(7, ErrorMessage = "Минимальная длинна поля 7 символов")]
        public required string Password { get; init; }

        [Required(ErrorMessage = "Укажите имя пользователя")]
        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public required string Firstname { get; set; }

        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public string? Lastname { get; init; }

        [MaxLength(250, ErrorMessage = "Максимальная длинна поля 250 символов")]
        public string? Discription { get; init; }

        [Required(ErrorMessage = "Укажите пол пользователя")]
        [Range(1, 2, ErrorMessage = "Указанное значение поля не поддерживается")]
        public required int Gender { get; init; }

        [Required(ErrorMessage = "Укажите город пользователя")]
        public required int City { get; init; }

        [Required(ErrorMessage = "Укажите IT профессию")]
        public required int ITProfession { get; init; }
    }
}
