using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;

namespace Shootsy.Models.File
{
    public class UpdateFileModel
    {
        [FromBody]
        public JsonPatchDocument<FileDto> PatchDocument { get; set; }

        [FromRoute]
        public int Id { get; set; }

        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
