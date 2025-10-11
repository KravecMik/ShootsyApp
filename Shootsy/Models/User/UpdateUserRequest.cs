using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;

namespace Shootsy.Models.User
{
    public class UpdateUserRequest
    {
        [FromRoute]
        public int IdUser { get; set; }

        [FromBody]
        public required JsonPatchDocument<UserDto> PatchDocument { get; set; }
    }
}