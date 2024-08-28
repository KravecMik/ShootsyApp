using Microsoft.AspNetCore.Mvc;

namespace Shootsy.Models
{
    public class GetUsersModel
    {
        [FromQuery(Name = "offset")]
        public int Offset { get; set; } = 0;

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 100;
    }
}
