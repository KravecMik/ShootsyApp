using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.User
{
    public class GetUserByIdRequest
    {
        [FromRoute(Name = "id")]
        public int IdUser { get; set; }
    }
}