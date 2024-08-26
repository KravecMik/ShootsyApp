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
        /// Логин
        /// </summary>
        string Login { get; }

        /// <summary>
        /// Пол
        /// </summary>
        int Gender { get; }

        /// <summary>
        /// Город
        /// </summary>
        int City { get; }

        /// <summary>
        /// Контакт для связи
        /// </summary>
        string Contact { get; }

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
        string Patronymic { get; }

        /// <summary>
        /// Полное имя
        /// </summary>
        string Fullname { get; }

        /// <summary>
        /// Описание
        /// </summary>
        string Discription { get; }

        /// <summary>
        /// Тип сотрудничества
        /// </summary>
        int CooperationType { get; }

        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        byte[] Password { get; }

        /// <summary>
        /// Тип пользователя
        /// </summary>
        int Type { get; }

        /// <summary>
        /// Съемка ню
        /// </summary>
        bool isNude { get; }

        /// <summary>
        /// Признак удаления пользователя
        /// </summary>
        bool isDelete { get; }

        /// <summary>
        /// Признак наличия активной подписки
        /// </summary>
        bool isHasActiveSubscribe { get; }

        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        DateTime? CreateDate { get; }

        /// <summary>
        /// Дата редактирования пользователя
        /// </summary>
        DateTime? EditDate { get; }
    }
}
