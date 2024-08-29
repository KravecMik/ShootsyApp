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
            var resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            return Ok(resultJson.RegexStringConverter());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] GetUserModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            if (user.Id is 0)
                return NotFound();

            var result = _mapper.Map<UserModelResponse>(user);
            var resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            return Ok(resultJson.RegexStringConverter());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync([FromRoute] GetUserModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.Id, cancellationToken);
            if (user.Id is 0)
                return NotFound();

            await _userRepository.DeleteByIdAsync(model.Id, cancellationToken);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(
            CreateUserModel model,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserDto>(model);
            var id = await _userRepository.CreateAsync(user, cancellationToken);

            return StatusCode(201, id);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync(
        UpdateUserModel model,
        CancellationToken cancellationToken = default)
        {
            var user = new UserDto { Id = model.Id };
            var operations = model.Operations.ToList();

            var updatedProperties = typeof(UserDto).GetPublicProperties()
                .Where(property => operations
                    .Any(operation => operation.path.EqualsPath(property)));


            await _userRepository.UpdateAsync(user, updatedProperties, cancellationToken);
            return NoContent();
        }
    }
}