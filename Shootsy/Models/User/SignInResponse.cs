namespace Shootsy.Models.User
{
    public class SignInResponse
    {
        public required int IdUser { get; set; }

        public required Guid Session { get; set; }
    }
}