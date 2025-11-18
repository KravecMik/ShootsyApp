using Microsoft.AspNetCore.Mvc;
using Shootsy.Database.Entities;
using Shootsy.Repositories;

namespace Shootsy.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<UserEntity?> GetCurrentUserIdAsync(IUserRepository userRepository, CancellationToken cancellationToken = default)
        {
            var sessionGuid = GetSessionGuidFromHeaders();
            return await userRepository.GetUserByGuidAsync(sessionGuid, cancellationToken);
        }

        protected Guid GetSessionGuidFromHeaders()
        {
            if (!Request.Headers.TryGetValue("Session", out var sessionHeader))
                throw new UnauthorizedAccessException("Session header is required");

            if (!Guid.TryParse(sessionHeader, out var sessionGuid))
                throw new ArgumentException("Invalid session format");

            return sessionGuid;
        }
    }
}