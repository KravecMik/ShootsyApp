using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;
using Shootsy.Models.User;

namespace Shootsy.Models
{
    public class UpdateUserModel : BaseUserModel
    {
        [FromBody]
        public JsonPatchDocument<UserDto> PatchDocument { get; set; }

        [FromRoute]
        public int Id { get; set; }
    }
}
