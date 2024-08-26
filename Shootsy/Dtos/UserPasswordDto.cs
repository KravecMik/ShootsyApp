namespace Shootsy.Dtos
{
    public class UserPasswordDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int IdUser { get; init; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public byte[] PasswordHash { get; init; }
    }
}
