using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.User
{
    public class GetUserByIdRequestModel
    {
        [FromRoute(Name = "id")]
        public int IdUser { get; set; }
    }
}