using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Database.Mongo;

namespace Shootsy.Models.File
{
    public class UpdateFileRequest
    {
        [FromRoute(Name = "id")]
        public required string IdFile { get; set; }

        [FromBody]
        public required JsonPatchDocument<FileStorageEntity> PatchDocument { get; set; }
    }
}
