using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Database.Mongo;

namespace Shootsy.Models.Post
{
    public class UpdatePostRequestModel
    {
        [FromRoute(Name = "id")]
        public required string IdPost { get; set; }

        [FromBody]
        public required JsonPatchDocument<PostEntity> PatchDocument { get; set; }
    }
}
