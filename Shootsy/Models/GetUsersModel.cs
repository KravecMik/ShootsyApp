using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models
{
    public class GetUsersModel
    {
        [FromQuery(Name = "offset")]
        public int Offset { get; set; }

        [FromQuery(Name = "limit")]
        [Required(ErrorMessage ="Укажите сколько записей необходимо вернуть")]
        public string Limit { get; set; }

        [FromQuery(Name = "filter")]
        public string Filter { get; set; } = "id > 0";

        [FromQuery(Name = "sort")]
        public string Sort { get; set; } = "id desc";
    }
}
