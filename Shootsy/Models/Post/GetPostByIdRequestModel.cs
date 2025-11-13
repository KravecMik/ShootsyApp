using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.Post
{
    public class GetPostByIdRequestModel
    {
        [FromRoute(Name = "id")]
        public required string IdPost { get; set; }
    }
}
