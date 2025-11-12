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
        /// Имя пользователя
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
        /// Имя
        /// </summary>
        string Firstname { get; }

        /// <summary>
        /// Фамилия
        /// </summary>
        string Lastname { get; }

        /// <summary>
        /// Описание
        /// </summary>
        string? Discription { get; }

        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        byte[] Password { get; }

        /// <summary>
        /// Тип специальности в IT
        /// </summary>
        int ITProfession { get; }
    }
}
