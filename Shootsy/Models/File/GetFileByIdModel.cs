using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class GetFileByIdModel : BaseFilesModel
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
