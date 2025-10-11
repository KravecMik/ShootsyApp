using Shootsy.Core.Interfaces;

namespace Shootsy.Database.Entities
{
    public class UserSessionEntity : IUserSession
    {
        public int Id { get; init; }
        public int User { get; init; }
        public DateTime SessionDateFrom { get; set; }
        public DateTime SessionDateTo { get; set; }
        public Guid Guid { get; init; }
        public UserEntity? UserEntity { get; init; }
    }
}
