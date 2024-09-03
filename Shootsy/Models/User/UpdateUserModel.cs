using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;

namespace Shootsy.Models
{
    public class UpdateUserModel
    {
        [FromBody]
        public JsonPatchDocument<UserDto> PatchDocument { get; set; }

        [FromRoute]
        public int Id { get; set; }

        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
