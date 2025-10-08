using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.User
{
    public class GetUsersModel : BaseUserModel
    {
        [FromQuery(Name = "offset")]
        public int Offset { get; set; }

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 100;

        [FromQuery(Name = "filter")]
        public string Filter { get; set; } = "id > 0";

        [FromQuery(Name = "sort")]
        public string Sort { get; set; } = "id desc";
    }
}
