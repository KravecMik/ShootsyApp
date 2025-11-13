using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.Post
{
    public class CreatePostRequestModel
    {
        [Required(ErrorMessage = "Укажите идентификатор пользователя")]
        public required int IdUser { get; init; }

        [MinLength(10, ErrorMessage = "Минимальная длинна поля 10 символов")]
        [MaxLength(250, ErrorMessage = "Максимальная длинна поля 250 символов")]
        public required string Text { get; init; }
    }
}
