using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Repositories;
using Shootsy.Security;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly SupportMethods _supportMethods;

        public UsersController(IUserRepository userRepository, IMapper mapper, SupportMethods supportMethods)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _supportMethods = supportMethods;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersModel model, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetListAsync(model.Limit, model.Offset, cancellationToken);
            var result = _mapper.Map<IEnumerable<UserModelResponse>>(users);
            var resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            return Ok(_supportMethods.RegexStringConverter(resultJson));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] GetUserModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            var result = _mapper.Map<UserModelResponse>(user);
            if (user.Id is 0)
                return NotFound();

            var resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            return Ok(_supportMethods.RegexStringConverter(resultJson));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] GetUserModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            var result = _mapper.Map<UserModelResponse>(user);
            if (user.Id is 0)
                return NotFound();

            await _userRepository.DeleteByIdAsync(model.Id, cancellationToken);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            CreateUserModel model,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var id = await _userRepository.CreateAsync(user, cancellationToken);

            await _userRepository.UpdateAsync(
                new UserDto { Id = id, Login = id.ToString() },
                new[] { nameof(UserDto.Login) },
                cancellationToken);

            return StatusCode(201, id);
        }
    }
}