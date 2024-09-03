using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Dtos;
using Shootsy.Models;
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
        private readonly IUserSessionRepository _userSessionRepository;

        public UsersController(IUserRepository userRepository, IMapper mapper, IUserSessionRepository userSessionRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userSessionRepository = userSessionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody, BindRequired]
            CreateUserModel model,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var existUsers = await _userRepository.GetByLoginAsync(model.Login, cancellationToken);
            if (existUsers != null)
                return BadRequest();

            var id = await _userRepository.CreateAsync(user, cancellationToken);
            var session = await _userSessionRepository.CreateAsync(id, cancellationToken);
            return StatusCode(201, session);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> SignInAsync(
            [FromBody, BindRequired]
            SignInModel model,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByLoginAsync(model.Login, cancellationToken);
            if (user is null)
                return BadRequest();

            var passwordVerification = model.Password.IsPasswordValid(model.Login, user.Password);
            if (!passwordVerification)
                return BadRequest();

            var session = await _userSessionRepository.CreateAsync(user.Id, cancellationToken);
            return StatusCode(200, session);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersModel model, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetListAsync(Convert.ToInt16(model.Limit), model.Offset, model.Filter, model.Sort, cancellationToken);
            var result = _mapper.Map<IEnumerable<UserModelResponse>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id, [FromHeader] string? session, CancellationToken cancellationToken = default)
        {
            if (session is null)
                return Unauthorized();

            var sessionGud = Guid.TryParse(session, out Guid res);
            if (!sessionGud)
                return Unauthorized();

            var isSessionActive = await _userSessionRepository.isSessionActive(res, cancellationToken);
            if (!isSessionActive)
                return Unauthorized();

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null)
                return NotFound();

            var result = _mapper.Map<UserModelResponse>(user);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserAsync(
        [FromBody] JsonPatchDocument<UserDto> patchDoc, [FromRoute] int id,
        CancellationToken cancellationToken = default)
        {
            var currentUser = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (currentUser is null)
                return NotFound();

            await _userRepository.UpdateAsync(currentUser, patchDoc, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null)
                return NotFound();

            await _userRepository.DeleteByIdAsync(id, cancellationToken);
            return NoContent();
        }
    }
}