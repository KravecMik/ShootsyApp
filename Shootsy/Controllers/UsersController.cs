using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Database.Entities;
using Shootsy.Models;
using Shootsy.Models.Dtos;
using Shootsy.Models.User;
using Shootsy.Repositories;
using Shootsy.Security;
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

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        [SwaggerOperation(Summary = "Регистрация пользователя")]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(SignUpRequestModel), typeof(SignUpRequestExampleModel))]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(SignUpResponseModel))]
        [SwaggerResponseExample(201, typeof(SignUpResponseExampleModel))]
        public async Task<IActionResult> SignUpAsync([FromBody, BindRequired] SignUpRequestModel request, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserEntity>(request);
            var existUsers = await _userRepository.GetUserByLoginAsync(request.Login, cancellationToken);
            if (existUsers != null)
            {
                ModelState.AddModelError("Login", $"Пользователь с таким логином уже существует");
                return ValidationProblem();
            }

            var id = await _userRepository.CreateUserAsync(user, cancellationToken);
            var session = await _userRepository.CreateSessionAsync(id, cancellationToken);

            return StatusCode(201, new SignUpResponseModel { UserId = id, Session = session });
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Авторизация пользователя")]
        [SwaggerRequestExample(typeof(SignInRequestModel), typeof(SignInRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(string))]
        [SwaggerResponseExample(200, typeof(string))]
        public async Task<IActionResult> SignInAsync([FromBody, BindRequired] SignInRequestModel request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLoginAsync(request.Login, cancellationToken);
            if (user is null) return NotFound();

            var passwordVerification = request.Password.IsPasswordValid(request.Login, user.Password);
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
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(IEnumerable<UserDto>))]
        [SwaggerResponseExample(200, typeof(GetUsersResponseExampleModel))]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UserFilterDto filter, CancellationToken cancellationToken = default)
        {
            var (usersEntities, totalCount) = await _userRepository.GetUsersListAsync(filter, cancellationToken);
            var users = _mapper.Map<IEnumerable<UserDto>>(usersEntities);

            var response = new PagedResponse<UserDto>
            {
                Data = users, 
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        [SwaggerOperation(Summary = "Получить данные пользователя по идентификатору")]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(UserDto))]
        [SwaggerResponseExample(200, typeof(GetUserByIdResponseExampleModel))]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
                return NotFound("Пользователь по указанному идентификатору не найден");

            var result = _mapper.Map<UserDto>(user);
            return Ok(result);
        }

        [Authorize]
        [HttpPatch("{userId:int}")]
        [SwaggerOperation(Summary = "Обновить данные пользователя по идентификатору")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] int userId, [FromBody] JsonPatchDocument<UserEntity> patch, CancellationToken cancellationToken = default)
        {
            var currentUser = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (currentUser is null)
                return NotFound("Пользователь по указанному идентификатору не найден");

            if (patch.Operations.Any(x => x.path.ToLower().Contains("login")))
            {
                ModelState.AddModelError("Login", "Поле логин недоступно для редактирования");
                return ValidationProblem();
            }

            await _userRepository.UpdateUserAsync(currentUser, patch, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{userId:int}")]
        [SwaggerOperation(Summary = "Удалить пользователя по идентификатору")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteUserByIdAsync([FromRoute] int userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
                return NotFound("Пользователь по указанному идентификатору id не найден");

            await _userRepository.DeleteUserByIdAsync(userId, cancellationToken);
            return NoContent();
        }
    }
}