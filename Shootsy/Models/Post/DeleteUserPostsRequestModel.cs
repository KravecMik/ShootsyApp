using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.Post
{
    public class DeleteUserPostsRequestModel
    {
        [FromRoute(Name = "id")]
        public required int IdUser { get; set; }
    }
}
