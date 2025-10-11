using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class GetFileByIdRequest
    {
        [FromRoute(Name = "id")]
        public required string IdFile { get; set; }
    }
}
