using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.User
{
    public class AuthorizationResponseModel
    {
        public int Id { get; set; }

        public Guid Session { get; set; }
    }
}
