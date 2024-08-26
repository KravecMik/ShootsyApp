using Shootsy.Core.Interfaces;

namespace Shootsy.Database.Entities
{
    public class UserEntity : IUser
    {
        public int Id { get; init; }

        public string? Login { get; init; }

        public int Gender { get; init; }

        public int City { get; init; }

        public string Contact { get; init; }

        public string Firstname { get; init; }

        public string Lastname { get; init; }

        public string Patronymic { get; init; }

        public string Fullname { get; init; }

        public string Discription { get; init; }

        public int CooperationType { get; init; }

        public byte[] Password { get; init; }

        public int Type { get; init; }

        public bool isNude { get; init; }

        public bool isDelete { get; init; }

        public bool isHasActiveSubscribe { get; init; }

        public DateTime? CreateDate { get; init; }

        public DateTime? EditDate { get; init; }

        public ICollection<UserSessionEntity> UserSessions { get; init; }

        public UserTypeEntity UserType { get; init; }
    }
}
