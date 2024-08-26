namespace Shootsy.Dtos
{
    /// <summary>
    /// Модель сессии пользователя
    /// </summary>
    public class UserSessionDto
    {
        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; init; }

        /// <summary>
        /// Дата начала сессии
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Guid сессии 
        /// </summary>
        public Guid Guid { get; init; } = Guid.NewGuid();
    }
}
