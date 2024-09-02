using Microsoft.AspNetCore.JsonPatch;
using Shootsy.Dtos;

namespace Shootsy.Repositories
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(UserDto user, CancellationToken cancellationToken);
        Task UpdateAsync(UserDto userDto, JsonPatchDocument<UserDto> jsonPatchDocument, CancellationToken cancellationToken = default);
        Task<UserDto>? GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto>? GetByLoginAsync(string login, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserDto>> GetListAsync(int limit, int offset, string filter, string sort, CancellationToken cancellationToken);
        Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
    }
}
