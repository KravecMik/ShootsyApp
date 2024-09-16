using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models.File
{
    public class DeleteManyFilesModel : BaseFilesModel
    {
        [FromQuery]
        public int[] Ids { get; set; }
    }
}
