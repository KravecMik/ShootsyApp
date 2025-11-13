using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Models.Message;
using Shootsy.Models.Message.Swagger;
using Shootsy.Repositories;
using Shootsy.Service;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Messages")]
    public class MessagesConroller : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IKafkaProducerService _kafkaProducer;

        public MessagesConroller(IUserRepository userRepository, IKafkaProducerService kafkaProducer)
        {
            _userRepository = userRepository;
            _kafkaProducer = kafkaProducer;
        }

        [Authorize]
        [HttpPost("send")]
        [SwaggerOperation(Summary = "Отправить сообщение пользователю по логину в кафку")]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(SendMessageRequestModel), typeof(SendMessageRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK")]
        public async Task<IActionResult> SendMessageByUserLoginAsync([FromBody, BindRequired] SendMessageRequestModel model, CancellationToken cancellationToken = default)
        {
            var toUser = await _userRepository.GetByLoginAsync(model.Login, cancellationToken);
            if (toUser is null) return NotFound();

            if (Request.Headers.TryGetValue("Session", out var sessionHeader))
            {
                var session = Request.Headers.Where(x => x.Key == "Session").Select(x => Guid.Parse(x.Value.ToString())).First();
                var fromUser = await _userRepository.GetUserIdByGuidAsync(session, cancellationToken);
                await _kafkaProducer.SendMessageByUserLoginAsync(fromUser.Login, model.Login, model.MessageText);
                return Ok();
            }
            else
            {
                ModelState.AddModelError("Session", "Не удалось получить идентификатор пользователя по заголовку Session");
                return ValidationProblem();
            }
        }
    }
}