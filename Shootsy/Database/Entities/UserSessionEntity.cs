namespace Shootsy.Database.Entities
{
    public class UserSessionEntity
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
        public DateTime CreateDate { get; init; }

        /// <summary>
        /// Guid сессии 
        /// </summary>
        public Guid Guid { get; init; }

        /// <summary>
        /// Сущность пользователя
        /// </summary>
        public UserEntity User { get; init; }
    }
}
