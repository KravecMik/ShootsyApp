using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Models.Message;
using Shootsy.Models.Message.Swagger;
using Shootsy.Repositories;
using Shootsy.Service;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Messages")]
    public class MessagesController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IKafkaProducerService _kafkaProducer;

        public MessagesController(IUserRepository userRepository, IKafkaProducerService kafkaProducer)
        {
            _userRepository = userRepository;
            _kafkaProducer = kafkaProducer;
        }

        [Authorize]
        [HttpPost("send/{userLogin}")]
        [SwaggerOperation(Summary = "Отправить сообщение в кафку по логину пользователя")]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(SendMessageRequestModel), typeof(SendMessageRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK")]
        public async Task<IActionResult> SendMessageByUserLoginAsync([FromRoute] string userLogin, [FromBody] SendMessageRequestModel model, CancellationToken cancellationToken = default)
        {
            var toUser = await _userRepository.GetUserByLoginAsync(userLogin, cancellationToken);
            if (toUser is null)
                return NotFound();

            var userId = await GetCurrentUserIdAsync(_userRepository, cancellationToken);
            await _kafkaProducer.SendMessageByUserLoginAsync(userId.Login, userLogin, model.Message);
            return Ok();
        }
    }
}