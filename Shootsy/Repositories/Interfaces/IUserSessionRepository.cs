using Shootsy.Dtos;

namespace Shootsy.Repositories
{
    public interface IUserSessionRepository
    {
        Task<Guid> CreateAsync(int userId, CancellationToken cancellationToken);
        Task<UserSessionDto>? GetByGuidAsync(Guid guid, CancellationToken cancellationToken);
        Task<bool> isSessionActive(Guid guid, CancellationToken cancellationToken);
    }
}
