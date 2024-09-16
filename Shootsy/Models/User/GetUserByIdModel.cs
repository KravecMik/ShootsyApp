using Microsoft.AspNetCore.Mvc;
using Shootsy.Models.User;

namespace Shootsy.Models
{
    public class GetUserByIdModel : BaseUserModel
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
