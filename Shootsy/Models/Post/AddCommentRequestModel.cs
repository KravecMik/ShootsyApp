using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.Post
{
    public class AddCommentRequestModel
    {
        [MinLength(1, ErrorMessage = "Минимальная длинна поля 1 символ")]
        [MaxLength(250, ErrorMessage = "Максимальная длинна поля 250 символов")]
        public string Text { get; set; } = string.Empty;
    }
}