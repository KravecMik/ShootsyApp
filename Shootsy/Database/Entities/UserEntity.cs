using Shootsy.Core.Interfaces;

namespace Shootsy.Database.Entities
{
    public class UserEntity : IUser
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public required string Login { get; set; }
        public required byte[] Password { get; init; }
        public int Gender { get; init; }
        public int City { get; init; }
        public string? Contact { get; init; }
        public required string Firstname { get; init; }
        public required string Lastname { get; init; }
        public string? Fullname { get; init; }
        public string? Discription { get; init; }
        public byte[]? Avatar { get; init; }
        public int Type { get; init; }
        public bool isNude { get; init; }
        public bool isEighteen { get; init; }
        public bool? isDelete { get; init; }

        public ICollection<UserSessionEntity>? UserSessionEntity { get; init; }
        public required UserTypeEntity UserTypeEntity { get; init; }
        public required CityEntity CityEntity { get; init; }
        public required GenderEntity GenderEntity { get; init; }
        public ICollection<UserRoleEntity>? UserRoleEntity { get; init; }
    }
}
