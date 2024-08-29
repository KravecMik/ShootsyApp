using AutoMapper.Configuration.Annotations;
using Shootsy.Core.Interfaces;

namespace Shootsy.Dtos
{
    public class UserDto : IUser
    {
        public int Id { get; init; }

        public string Firstname { get; init; }

        public string Lastname { get; init; }

        public string Login { get; init; }

        public string Contact { get; init; }

        public byte[] Password { get; init; }

        public string? Patronymic { get; init; }

        public string Fullname { get; init; }

        public string? Discription { get; init; }

        public int CooperationType { get; init; }

        public int Type { get; init; }

        public bool? isNude { get; init; }

        public int City { get; init; }

        public bool? isHasActiveSubscribe { get; init; }

        public int Gender { get; init; }

        public DateTime? CreateDate { get; init; }

        public DateTime? EditDate { get; init; }
    }
}
