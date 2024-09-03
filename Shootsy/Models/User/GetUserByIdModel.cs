using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models
{
    public class GetUserByIdModel
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }

        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
