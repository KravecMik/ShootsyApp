using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.HttpSys;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Models;
using Shootsy.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Users")]
    //[Authorize(AuthenticationSchemes = nameof(AuthenticationSchemes.Basic))]
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
            var users = await _userRepository.GetListAsync(model.Limit, model.Offset, model.Sort, cancellationToken);

            var result = _mapper.Map<IEnumerable<UserModelResponse>>(users);
            return Ok(result);
        }

        //// POST: User/Create
        //[HttpPost]
        //[Consumes(MediaTypeNames.Application.Json)]
        //public async Task<IActionResult> CreateAsync(
        //    CreateUserModel model,
        //    CancellationToken cancellationToken = default)
        //{
        //    var user = UserDto > (model);
        //    var id = await _userRepository.CreateAsync(user, cancellationToken);

        //    await _userRepository.UpdateAsync(
        //        new UserDto { id = id, Login = id.ToString() }),
        //        new[] { nameof(UserDto.Login) },
        //        cancellationToken);

        //    return StatusCode(201, id);
        //}

    }
}
