using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Dtos;
using Shootsy.Models.User;
using Shootsy.Repositories;
using Shootsy.Security;
using Shootsy.Service;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly InternalConstants _internalConstants;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly HttpClient _httpClient;

        public UsersController(IUserRepository userRepository, IMapper mapper, HttpClient httpClient, InternalConstants internalConstants, IKafkaProducerService kafkaProducer)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _internalConstants = internalConstants;
            _kafkaProducer = kafkaProducer;
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(Summary = "Регистрация пользователя")]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(SignUpRequest), typeof(SignUpRequestExample))]
        [SwaggerResponse(statusCode: 201, description: "OK", type: typeof(SignInResponse))]
        [SwaggerResponseExample(201, typeof(SignInResponseExample))]
        public async Task<IActionResult> SignUpAsync([FromBody, BindRequired] SignUpRequest model, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var existUsers = await _userRepository.GetByLoginAsync(model.Login, cancellationToken);
            if (existUsers != null)
            {
                await _kafkaProducer.ProduceUserEventAsync("user.error", $"Пользователь с таким Login: {model.Login} уже существует");
                return StatusCode(400, $"Пользователь с таким Login: {model.Login} уже существует");
            }

            var id = await _userRepository.CreateAsync(user, cancellationToken);
            var session = await _userRepository.CreateSessionAsync(id, cancellationToken);

            await _kafkaProducer.ProduceUserEventAsync("user.created", new { id, user.Login });
            return StatusCode(201, new SignInResponse { IdUser = id, Session = session });
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Авторизация пользователя")]
        [SwaggerRequestExample(typeof(SignInRequest), typeof(SignInRequestExample))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(SignInResponse))]
        [SwaggerResponseExample(200, typeof(SignInResponseExample))]
        public async Task<IActionResult> SignInAsync([FromBody, BindRequired] SignInRequest model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByLoginAsync(model.Login, cancellationToken);
            if (user is null)
                return NotFound("Пользователь не найден");

            var passwordVerification = model.Password.IsPasswordValid(model.Login, user.Password);
            if (!passwordVerification)
                return StatusCode(400, "Неверно указан логин или пароль пользователя");

            var session = await _userRepository.CreateSessionAsync(user.Id, cancellationToken);
            await _kafkaProducer.ProduceUserEventAsync("user.authorized", $"Пользователь с логином: {model.Login} успешно авторизовался");
            return StatusCode(200, session);
        }

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список пользователей")]
        [SwaggerRequestExample(typeof(GetUsersRequest), typeof(GetUsersRequestExample))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(IEnumerable<GetUserByIdResponse>))]
        [SwaggerResponseExample(200, typeof(GetUsersResponseExample))]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersRequest model, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetListAsync(model.Limit, model.Offset, model.Filter, model.Sort, cancellationToken);
            var result = _mapper.Map<IEnumerable<GetUserByIdResponse>>(users);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить данные пользователя по идентификатору")]
        [SwaggerRequestExample(typeof(GetUserByIdRequest), typeof(GetUserByIdRequestExample))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(GetUserByIdResponse))]
        [SwaggerResponseExample(200, typeof(GetUserByIdResponseExample))]
        public async Task<IActionResult> GetUserByIdAsync([FromQuery] GetUserByIdRequest model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (user is null)
                return NotFound("Пользователь по указанному id не найден");

            var result = _mapper.Map<GetUserByIdResponse>(user);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id}")]
        [SwaggerOperation(Summary = "Обновить данные пользователя по идентификатору")]
        [SwaggerRequestExample(typeof(UpdateUserRequest), typeof(UpdateUserRequestExample))]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequest model, CancellationToken cancellationToken = default)
        {
            var currentUser = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (currentUser is null)
                return NotFound("Пользователь по указанному id не найден");

            var isExistOperationWithLogin = model.PatchDocument.Operations.Any(x => x.path.ToLower().Contains("Login"));
            if (isExistOperationWithLogin)
            {
                return StatusCode(400, "Поле 'Login' недоступно для редактирования");
            }

            await _userRepository.UpdateAsync(currentUser, model.PatchDocument, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить пользователя по идентификатору")]
        [SwaggerRequestExample(typeof(GetUserByIdRequest), typeof(GetUserByIdRequestExample))]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteUserByIdAsync([FromQuery] GetUserByIdRequest model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (user is null)
                return NotFound("Пользователь по указанному id не найден");

            await _userRepository.DeleteByIdAsync(model.IdUser, cancellationToken);
            return NoContent();
        }
    }
}