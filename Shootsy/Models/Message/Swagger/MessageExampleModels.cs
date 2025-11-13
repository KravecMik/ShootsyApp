using Shootsy.Models.Post;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.Message.Swagger
{
    public class SendMessageRequestExampleModel : IExamplesProvider<SendMessageRequestModel>
    {
        public SendMessageRequestModel GetExamples() => new SendMessageRequestModel
        {
            Login = "DarkStalker",
            MessageText = "Я шлю тебе письмо прям в кафку!!!"
        };
    }
}
