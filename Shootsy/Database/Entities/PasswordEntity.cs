namespace Shootsy.Database.Entities
{
    public class PasswordEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Password { get; set; }

        public int Salt { get; init; }

        public bool isActive { get; init; }

        /// <summary>
        /// Сущность пользователя
        /// </summary>
        public UserEntity Users { get; set; } = new();
    }
}
