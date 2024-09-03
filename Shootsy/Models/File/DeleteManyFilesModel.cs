using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class DeleteManyFilesModel
    {
        [FromQuery]
        public int[] Ids { get; set; }

        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
