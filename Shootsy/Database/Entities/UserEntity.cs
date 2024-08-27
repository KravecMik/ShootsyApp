using AutoMapper.Configuration.Annotations;
using Shootsy.Core.Interfaces;

namespace Shootsy.Database.Entities
{
    public class UserEntity : IUser
    {
        public int Id { get; init; }

        public string? Login { get; init; }

        public int GenderId { get; init; }

        public int CityId { get; init; }

        public string? Contact { get; init; }

        [Ignore]
        public int Salt { get; set; }

        public string Firstname { get; init; }

        public string? Lastname { get; init; }

        public string? Patronymic { get; init; }

        public string? Fullname { get; init; }

        public string? Discription { get; init; }

        public int? CooperationTypeId { get; init; }

        public int PasswordId { get; init; }

        public int? TypeId { get; init; }

        public bool? isNude { get; init; }

        public bool? isDelete { get; init; }

        public bool? isHasActiveSubscribe { get; init; }

        public DateTime? CreateDate { get; set; }

        public DateTime? EditDate { get; set; }

        public ICollection<UserSessionEntity>? UserSessions { get; init; }

        public UserTypeEntity UserType { get; init; }

        public CityEntity City { get; init; }

        public GenderEntity Gender { get; init; }

        public CooperationTypeEntity CooperationType { get; init; }

        public ICollection<PasswordEntity> Password { get; init; }
    }
}
