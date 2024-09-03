using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class GetFileByIdModel
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }

        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
