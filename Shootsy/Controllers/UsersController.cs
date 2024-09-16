using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Models.User;
using Shootsy.Repositories;
using Shootsy.Security;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        InternalConstants _internalConstants;
        private readonly HttpClient _httpClient;

        public UsersController(IUserRepository userRepository, IMapper mapper, HttpClient httpClient, InternalConstants internalConstants)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _internalConstants = internalConstants;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody, BindRequired]
            CreateUserModel model,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var existUsers = await _userRepository.GetByUsernameAsync(model.Username, cancellationToken);
            if (existUsers != null)
                return StatusCode(400, "Пользователь с таким Username уже существует");

            var id = await _userRepository.CreateAsync(user, cancellationToken);
            var session = await _userRepository.CreateSessionAsync(id, cancellationToken);
            return StatusCode(201, session);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthorizationAsync(
            [FromBody, BindRequired]
            AuthorizationModel model,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByUsernameAsync(model.Username, cancellationToken);
            if (user is null)
                return NotFound("Пользователь по указанному Username не найден");

            var passwordVerification = model.Password.IsPasswordValid(model.Username, user.Password);
            if (!passwordVerification)
                return StatusCode(400, "Неверно указан Username или пароль пользователя");

            var session = await _userRepository.CreateSessionAsync(user.Id, cancellationToken);
            return StatusCode(200, session);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(GetUsersModel model, CancellationToken cancellationToken = default)
        {
            var isAuthorized = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isAuthorized)
                return StatusCode(401, "Пользователь не авторизован");

            var users = await _userRepository.GetListAsync(model.Limit, model.Offset, model.Filter, model.Sort, cancellationToken);
            var result = _mapper.Map<IEnumerable<UserModelResponse>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(GetUserByIdModel model, CancellationToken cancellationToken = default)
        {
            var isAuthorized = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isAuthorized)
                return StatusCode(401, "Пользователь не авторизован");

            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            if (user is null)
                return NotFound("Пользователь по указанному id не найден");

            var result = _mapper.Map<UserModelResponse>(user);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserAsync(
        UpdateUserModel model,
        CancellationToken cancellationToken = default)
        {
            var isAuthorized = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isAuthorized)
                return StatusCode(401, "Пользователь не авторизован");

            var isForbidden = await _httpClient.GetAsync($"{_internalConstants.BaseUrl}/users/session/{model.Session}/access-to/{model.Id}");
            if (!isForbidden.IsSuccessStatusCode)
                return StatusCode(403, "У пользователя недостаточно прав");

            var currentUser = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            if (currentUser is null)
                return NotFound("Пользователь по указанному id не найден");

            var isExistOperationWithUsername = model.PatchDocument.Operations.Any(x => x.path.ToLower().Contains("username"));
            if (isExistOperationWithUsername)
            {
                return StatusCode(400, "Поле 'Username' недоступно для редактирования");
            }

            await _userRepository.UpdateAsync(currentUser, model.PatchDocument, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(GetUserByIdModel model, CancellationToken cancellationToken = default)
        {
            var isAuthorized = await _userRepository.IsAuthorized(model.Session, cancellationToken);
            if (!isAuthorized)
                return StatusCode(401, "Пользователь не авторизован");

            var isForbidden = await _httpClient.GetAsync($"{_internalConstants.BaseUrl}/users/session/{model.Session}/access-to/{model.Id}");
            if (!isForbidden.IsSuccessStatusCode)
                return StatusCode(403, "У пользователя недостаточно прав");

            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            if (user is null)
                return NotFound("Пользователь по указанному id не найден");

            await _userRepository.DeleteByIdAsync(model.Id, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}/session/last")]
        public async Task<IActionResult> GetLastUserSessionAsync(GetUserByIdModel model, CancellationToken cancellationToken = default)
        {
            var lastSession = await _userRepository.GetLastSessionAsync(model.Id, cancellationToken);
            if (lastSession == Guid.Empty)
                return NotFound("Пользователь по указанной сессии не найден");
            return StatusCode(200, lastSession);
        }

        [HttpGet("session/{session}/access-to/{needAccessToId}")]
        public async Task<IActionResult> CheckUserAccessAsync([FromRoute] string session, [FromRoute] int needAccessToId, CancellationToken cancellationToken = default)
        {
            var isHaveAccess = await _userRepository.IsHaveAccessToIdAsync(session, needAccessToId, cancellationToken);
            if (!isHaveAccess)
                return StatusCode(403, "У пользователя недостаточно прав");
            return StatusCode(204);
        }

        [HttpGet("{username}/is-available")]
        public async Task<IActionResult> CheckIsUsernameAvailableAsync([FromRoute] string username, CancellationToken cancellationToken = default)
        {
            var existUsers = await _userRepository.GetByUsernameAsync(username, cancellationToken);
            if (existUsers is not null)
                return StatusCode(200, "false");
            return StatusCode(200, "true");
        }
    }
}