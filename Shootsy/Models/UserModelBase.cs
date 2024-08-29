namespace Shootsy.Models
{
    public class UserModelResponseBase
    {
        public int Id { get; init; }

        public string Login { get; init; }

        public int Gender { get; init; }

        public int City { get; init; }

        public string Firstname { get; init; }

        public string Lastname { get; init; }

        public int CooperationType { get; init; }

        public int Type { get; init; }
    }
}
