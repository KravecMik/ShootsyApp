using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class DeleteUserFilesRequestModel
    {
        [FromRoute(Name = "id")]
        public required int IdUser { get; set; }
    }
}
