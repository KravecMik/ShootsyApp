namespace Shootsy.Models.User
{
    public class SignUpResponseModel
    {
        public required int IdUser { get; set; }

        public required Guid Session { get; set; }
    }
}