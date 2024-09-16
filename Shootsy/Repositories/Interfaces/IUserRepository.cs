using Microsoft.AspNetCore.JsonPatch;
using Shootsy.Dtos;

namespace Shootsy.Repositories
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(UserDto user, CancellationToken cancellationToken);
        Task<UserDto>? GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto>? GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<UserDto>? GetByGuidAsync(Guid guid, CancellationToken cancellationToken);
        Task<Guid> GetLastSessionAsync(int userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserDto>> GetListAsync(int limit, int offset, string filter, string sort, CancellationToken cancellationToken);
        Task UpdateAsync(UserDto userDto, JsonPatchDocument<UserDto> jsonPatchDocument, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(int id, CancellationToken cancellationToken);



        Task<Guid> CreateSessionAsync(int userId, CancellationToken cancellationToken);
        Task<UserSessionDto>? GetSessionByGuidAsync(Guid guid, CancellationToken cancellationToken);
        Task<bool> IsAuthorized(string? guid, CancellationToken cancellationToken);
        Task<bool> IsHaveAccessToIdAsync(string session, int needAccsessToId, CancellationToken cancellationToken);
    }
}
