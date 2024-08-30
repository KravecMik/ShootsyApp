using Microsoft.AspNetCore.JsonPatch.Operations;
using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models
{
    public class UpdateUserModel
    {
        [Required(ErrorMessage = "Укажите Id редактируемого пользователя")]
        public int Id { get; init; }

        [Required(ErrorMessage = "Укажите операции над пользователем")]
        public IEnumerable<Operation> Operations { get; init; }
    }
}
