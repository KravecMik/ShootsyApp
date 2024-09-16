namespace Shootsy.Core.Interfaces
{
    public interface IUserSession
    {
        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        int Id { get; init; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        int User { get; init; }

        /// <summary>
        /// Дата начала сессии
        /// </summary>
        DateTime SessionDateFrom { get; set; }

        /// <summary>
        /// Дата окончания сессии
        /// </summary>
        DateTime SessionDateTo { get; set; }

        /// <summary>
        /// Guid сессии 
        /// </summary>
        Guid Guid { get; init; }
    }
}
