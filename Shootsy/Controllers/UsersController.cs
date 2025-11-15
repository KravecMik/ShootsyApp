using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly HttpClient _httpClient;

        public UsersController(IUserRepository userRepository, IMapper mapper, HttpClient httpClient, InternalConstants internalConstants)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _internalConstants = internalConstants;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        [SwaggerOperation(Summary = "Регистрация пользователя")]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(SignUpRequestModel), typeof(SignUpRequestExampleModel))]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(SignUpResponseModel))]
        [SwaggerResponseExample(201, typeof(SignUpResponseExampleModel))]
        public async Task<IActionResult> SignUpAsync([FromBody, BindRequired] SignUpRequestModel model, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var existUsers = await _userRepository.GetByLoginAsync(model.Login, cancellationToken);
            if (existUsers != null)
            {
                ModelState.AddModelError("Login", $"Пользователь с таким логином уже существует");
                return ValidationProblem();
            }

            var id = await _userRepository.CreateAsync(user, cancellationToken);
            var session = await _userRepository.CreateSessionAsync(id, cancellationToken);

            return StatusCode(201, new SignUpResponseModel { IdUser = id, Session = session });
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Авторизация пользователя")]
        [SwaggerRequestExample(typeof(SignInRequestModel), typeof(SignInRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(string))]
        [SwaggerResponseExample(200, typeof(string))]
        public async Task<IActionResult> SignInAsync([FromBody, BindRequired] SignInRequestModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByLoginAsync(model.Login, cancellationToken);
            if (user is null) return NotFound();

            var passwordVerification = model.Password.IsPasswordValid(model.Login, user.Password);
            if (!passwordVerification)
            {
                ModelState.AddModelError("detail", "Неверно указан логин или пароль пользователя");
                return ValidationProblem();
            }    

            var session = await _userRepository.CreateSessionAsync(user.Id, cancellationToken);
            return Content(session.ToString(), "text/plain");
        }

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список пользователей")]
        [SwaggerRequestExample(typeof(GetUsersRequestModel), typeof(GetUsersRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(IEnumerable<GetUserByIdResponse>))]
        [SwaggerResponseExample(200, typeof(GetUsersResponseExampleModel))]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersRequestModel model, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetListAsync(model.Limit, model.Offset, model.Filter, model.Sort, cancellationToken);
            var result = _mapper.Map<IEnumerable<GetUserByIdResponse>>(users);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить данные пользователя по идентификатору")]
        [SwaggerRequestExample(typeof(GetUserByIdRequestModel), typeof(GetUserByIdRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(GetUserByIdResponse))]
        [SwaggerResponseExample(200, typeof(GetUserByIdResponseExampleModel))]
        public async Task<IActionResult> GetUserByIdAsync([FromQuery] GetUserByIdRequestModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (user is null) return NotFound("Пользователь по указанному идентификатору не найден");

            var result = _mapper.Map<GetUserByIdResponse>(user);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{id}")]
        [SwaggerOperation(Summary = "Обновить данные пользователя по идентификатору")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdateUserAsync(
            int id,
            [FromBody] JsonPatchDocument<UserDto> patchDocument,
            CancellationToken cancellationToken = default)
        {
            var currentUser = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (currentUser is null)
                return NotFound("Пользователь по указанному идентификатору не найден");

            if (patchDocument.Operations.Any(x => x.path.ToLower().Contains("login")))
            {
                ModelState.AddModelError("Login", "Поле логин недоступно для редактирования");
                return ValidationProblem();
            }

            await _userRepository.UpdateAsync(currentUser, patchDocument, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить пользователя по идентификатору")]
        [SwaggerRequestExample(typeof(GetUserByIdRequestModel), typeof(GetUserByIdRequestExampleModel))]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteUserByIdAsync([FromQuery] GetUserByIdRequestModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (user is null) return NotFound("Пользователь по указанному идентификатору id не найден");

            await _userRepository.DeleteByIdAsync(model.IdUser, cancellationToken);
            return NoContent();
        }
    }
}