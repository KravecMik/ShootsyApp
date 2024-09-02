using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Repositories;

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

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody, BindRequired]
            CreateUserModel model,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var existUsers = await _userRepository.GetListAsync(100, 0, cancellationToken);
            if (existUsers.Any(x => x.Login.Contains(model.Login)))
                return BadRequest();

            var id = await _userRepository.CreateAsync(user, cancellationToken);
            return StatusCode(201, id);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersModel model, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetListAsync(model.Limit, model.Offset, cancellationToken);
            var result = _mapper.Map<IEnumerable<UserModelResponse>>(users);
            return Ok(result.OrderByDescending(x => x.Id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
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