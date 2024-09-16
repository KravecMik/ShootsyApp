using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.User
{
    public class BaseUserModel
    {
        [FromHeader(Name = "session")]
        public string? Session { get; set; }
    }
}
