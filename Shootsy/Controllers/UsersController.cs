using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Repositories;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersModel model, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetListAsync(model.Limit, model.Offset, cancellationToken);
            var result = _mapper.Map<IEnumerable<UserModelResponse>>(users);
            return Ok(result.OrderByDescending(x => x.Id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] GetUserModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            if (user is null)
                return NotFound();

            var result = _mapper.Map<UserModelResponse>(user);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync([FromRoute] GetUserModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            if (user is null)
                return NotFound();

            await _userRepository.DeleteByIdAsync(model.Id, cancellationToken);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody, BindRequired]
            CreateUserModel model,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var id = await _userRepository.CreateAsync(user, cancellationToken);

            return StatusCode(201, id);
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
    }
}