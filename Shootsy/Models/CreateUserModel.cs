using Shootsy.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models
{
    [SwaggerSchema("Команда на создание пользователя")]
    public class CreateUserModel
    {
        [Required]
        [SwaggerSchema("Логин")]
        public string Usermame { get; set; }

        [Required]
        [SwaggerSchema("Пароль")]
        public byte[] Password { get; set; }

        [SwaggerSchema("Контакт")]
        public string Contact { get; set; }

        [Required]
        [SwaggerSchema("Имя")]
        public string Firstname { get; set; }

        [Required]
        [SwaggerSchema("Фамилия")]
        public string Lastname { get; set; }

        [SwaggerSchema("Отчество")]
        public string Patronymic { get; set; }

        [SwaggerSchema("Описание профиля")]
        public string Discription { get; set; }

        [Required]
        [SwaggerSchema("Условие сотрудничества")]
        public CooperationTypeEnum CooperationType { get; set; }

        [Required]
        [SwaggerSchema("Роль")]
        public TypeEnum Role { get; set; }

        [Required]
        [SwaggerSchema("Работа с НЮ съемками")]
        public bool Nude { get; set; }
    }
}
