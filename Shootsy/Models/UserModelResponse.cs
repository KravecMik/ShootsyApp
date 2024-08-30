using Shootsy.Core.Interfaces;

namespace Shootsy.Models
{
    public class UserModelResponse
    {
        public int Id { get; init; }

        public string Login { get; init; }

        public string Gender { get; init; }

        public string City { get; init; }

        public string Firstname { get; init; }

        public string Lastname { get; init; }

        public string CooperationType { get; init; }

        public string Type { get; init; }

        public string? Contact { get; init; }

        public string? Patronymic { get; init; }

        public string? Fullname { get; init; }

        public string? Discription { get; init; }

        public bool? isNude { get; init; }

        public bool? isDelete { get; init; }

        public DateTime? CreateDate { get; init; }

        public DateTime? EditDate { get; init; }
    }
}
