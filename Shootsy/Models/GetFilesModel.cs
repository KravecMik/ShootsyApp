using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models
{
    public class GetFilesModel
    {
        [FromQuery(Name = "offset")]
        public int Offset { get; set; } = 0;

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 100;

        [FromQuery(Name = "userId")]
        public int? UserId { get; set; }
    }
}
