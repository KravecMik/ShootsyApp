using Shootsy.Core.Interfaces;

namespace Shootsy.Models
{
    public class CreateUserModel
    {
        public string Login { get; init; }

        public byte[]? Password { get; init; }

        public string Contact { get; init; }

        public string Firstname { get; init; }

        public string? Lastname { get; init; }

        public string? Patronymic { get; init; }

        public string? Discription { get; init; }

        public int? CooperationTypeId { get; init; } = 1;

        public bool? isNude { get; init; } = false;

        public int GenderId { get; init; }

        public int CityId { get; init; }

        public int TypeId { get; init; }

        public bool? isHasActiveSubscribe { get; init; } = false;
    }
}
