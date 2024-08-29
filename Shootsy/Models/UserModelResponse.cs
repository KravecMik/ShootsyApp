using Shootsy.Core.Interfaces;

namespace Shootsy.Models
{
    public class UserModelResponse : UserModelResponseBase
    {
        public string? Contact { get; init; }

        public string? Patronymic { get; init; }

        public string? Fullname { get; init; }

        public string? Discription { get; init; }

        public bool? isNude { get; init; }

        public bool? isDelete { get; init; }

        public bool? isHasActiveSubscribe { get; init; }

        public DateTime? CreateDate { get; init; }

        public DateTime? EditDate { get; init; }
    }
}
