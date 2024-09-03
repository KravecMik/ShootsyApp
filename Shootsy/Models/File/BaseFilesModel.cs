using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class BaseFilesModel
    {
        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
