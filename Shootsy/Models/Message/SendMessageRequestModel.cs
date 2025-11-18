using System.ComponentModel.DataAnnotations;

namespace Shootsy.Models.Message
{
    public class SendMessageRequestModel
    {
        [Required(ErrorMessage = "Укажите текст сообщения")]
        [MinLength(10, ErrorMessage = "Минимальная длинна поля 10 символов")]
        [MaxLength(250, ErrorMessage = "Максимальная длинна поля 250 символов")]
        public required string Message { get; set; }
    }
}