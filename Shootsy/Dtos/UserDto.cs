using Shootsy.Core.Interfaces;

namespace Shootsy.Dtos
{
    public class UserDto : IUser
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime EditDate { get; init; }
        public required string Firstname { get; init; }
        public string? Lastname { get; init; }
        public required string Login { get; set; }
        public required byte[] Password { get; init; }
        public string? Discription { get; init; }
        public int ITProfession { get; init; }
        public int City { get; init; }
        public int Gender { get; init; }
    }
}
