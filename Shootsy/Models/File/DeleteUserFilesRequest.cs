using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class DeleteUserFilesRequest
    {
        [FromRoute(Name = "id")]
        public required int IdUser { get; set; }
    }
}
