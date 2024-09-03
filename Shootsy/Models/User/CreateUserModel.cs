using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Укажите логин пользователя")]
        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public string Login { get; init; }

        [Required(ErrorMessage = "Укажите пароль пользователя")]
        [MinLength(7, ErrorMessage = "Минимальная длинна поля 7 символов")]
        public string Password { get; init; }

        [MaxLength(100, ErrorMessage = "Максимальная длинна поля 100 символов")]
        public string? Contact { get; init; }

        [Required(ErrorMessage = "Укажите имя пользователя")]
        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Укажите фамилию пользователя")]
        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public string Lastname { get; init; }

        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 50 символов")]
        public string? Patronymic { get; init; }

        [MaxLength(50, ErrorMessage = "Максимальная длинна поля 250 символов")]
        public string? Discription { get; init; }

        [Required(ErrorMessage = "Укажите пол пользователя")]
        [Range(1, 2, ErrorMessage = "Указанное значение поля не поддерживается")]
        public int Gender { get; init; }

        [Required(ErrorMessage = "Укажите город пользователя")]
        [Range(1, 2, ErrorMessage = "Указанное значение поля не поддерживается")]
        public int City { get; init; }

        [Required(ErrorMessage = "Укажите тип пользователя")]
        [Range(1, 2, ErrorMessage = "Указанное значение поля не поддерживается")]
        public int Type { get; init; }

        [Range(1, 4, ErrorMessage = "Указанное значение поля не поддерживается")]
        public int? CooperationType { get; init; } = 1;

        public bool? isNude { get; init; } = false;

        public byte[]? Avatar { get; init; }

    }
}
