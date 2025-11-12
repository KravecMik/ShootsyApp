namespace Shootsy.Models.User
{
    public class GetUserByIdResponse
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime EditDate { get; init; }
        public required string Login { get; init; }
        public required string Firstname { get; init; }
        public string? Lastname { get; init; }
        public required string Gender { get; init; }
        public required string City { get; init; }
        public required string ITProfession { get; init; }
        public string? Discription { get; init; }
    }
}
