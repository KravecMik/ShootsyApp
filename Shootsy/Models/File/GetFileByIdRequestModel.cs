using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class GetFileByIdRequestModel
    {
        [FromRoute(Name = "id")]
        public required string IdFile { get; set; }
    }
}
