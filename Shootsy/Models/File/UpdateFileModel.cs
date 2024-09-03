using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;

namespace Shootsy.Models.File
{
    public class UpdateFileModel : BaseFilesModel
    {
        [FromBody]
        public JsonPatchDocument<FileDto> PatchDocument { get; set; }

        [FromRoute]
        public int Id { get; set; }
    }
}
