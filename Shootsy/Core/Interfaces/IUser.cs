namespace Shootsy.Core.Interfaces
{
    /// <summary>
    /// Единый интерфейс пользователя
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        DateTime CreateDate { get; }

        /// <summary>
        /// Дата редактирования пользователя
        /// </summary>
        DateTime EditDate { get; }

        /// <summary>
        /// Логин
        /// </summary>
        string Login { get; }

        /// <summary>
        /// Идентификатор пола
        /// </summary>
        int Gender { get; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        int City { get; }

        /// <summary>
        /// Контакт для связи
        /// </summary>
        string? Contact { get; }

        /// <summary>
        /// Имя
        /// </summary>
        string Firstname { get; }

        /// <summary>
        /// Фамилия
        /// </summary>
        string Lastname { get; }

        /// <summary>
        /// Отчество
        /// </summary>
        string? Patronymic { get; }

        /// <summary>
        /// Полное имя
        /// </summary>
        string? Fullname { get; }

        /// <summary>
        /// Описание
        /// </summary>
        string? Discription { get; }

        /// <summary>
        /// Идентификатор типа сотрудничества
        /// </summary>
        int CooperationType { get; }

        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        byte[] Password { get; }

        /// <summary>
        /// Аватар пользователя
        /// </summary>
        byte[]? Avatar { get; }

        /// <summary>
        /// Идентификатор типа пользователя
        /// </summary>
        int Type { get; }

        /// <summary>
        /// Съемка ню
        /// </summary>
        bool? isNude { get; }
    }
}
