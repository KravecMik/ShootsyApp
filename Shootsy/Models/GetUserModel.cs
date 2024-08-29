using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models
{
    public class GetUserModel
    {
        [FromRoute(Name = "id")]
        public required int Id { get; set; }
    }
}
