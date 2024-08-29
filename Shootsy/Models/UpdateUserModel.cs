using Microsoft.AspNetCore.JsonPatch.Operations;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models
{
    public class UpdateUserModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public IEnumerable<Operation> Operations { get; set; }
    }
}
