namespace Shootsy.Models.User
{
    public class SignUpResponseModel
    {
        public required int UserId { get; set; }

        public required Guid Session { get; set; }
    }
}