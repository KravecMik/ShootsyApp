using Microsoft.AspNetCore.JsonPatch;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;

namespace Shootsy.Repositories
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(UserEntity user, CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByLoginAsync(string userLogin, CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken);
        Task<Guid?> GetLastSessionAsync(int userId, CancellationToken cancellationToken);
        Task<(IReadOnlyList<UserEntity>, int)> GetUsersListAsync(UserFilterDto filter, CancellationToken cancellationToken);
        Task UpdateUserAsync(UserEntity userDto, JsonPatchDocument<UserEntity> jsonPatchDocument, CancellationToken cancellationToken);
        Task DeleteUserByIdAsync(int userId, CancellationToken cancellationToken);
        Task<Guid> CreateSessionAsync(int userId, CancellationToken cancellationToken);
        Task<UserSessionEntity?> GetSessionByGuidAsync(Guid guid, CancellationToken cancellationToken);
        Task<bool> IsAuthorizedAsync(string? guid, CancellationToken cancellationToken);
    }
}