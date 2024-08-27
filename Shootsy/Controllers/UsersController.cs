using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shootsy.Dtos;
using Shootsy.Models;
using Shootsy.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Users")]
    //[Authorize(AuthenticationSchemes = nameof(AuthenticationSchemes.Basic))]
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
            return Ok(resultJson);
        }

        // POST: User/Create
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
