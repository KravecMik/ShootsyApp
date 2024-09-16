using Shootsy.Core.Interfaces;

namespace Shootsy.Dtos
{
    public class UserDto : IUser
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime EditDate { get; init; }
        public string Firstname { get; init; }
        public string Lastname { get; init; }
        public string Username { get; set; }
        public string Contact { get; init; }
        public byte[] Password { get; init; }
        public byte[]? Avatar { get; init; }
        public string Fullname { get; init; }
        public string? Discription { get; init; }
        public int Type { get; init; }
        public bool isNude { get; init; }
        public bool isEighteen { get; init; }
        public int City { get; init; }
        public int Gender { get; init; }
    }
}
