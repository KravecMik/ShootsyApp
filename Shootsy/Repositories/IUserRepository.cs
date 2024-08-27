using Shootsy.Dtos;

namespace Shootsy.Repositories
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(UserDto user, CancellationToken cancellationToken);
        Task UpdateAsync(UserDto user, IEnumerable<string> updateProperties, CancellationToken cancellationToken);
        Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserDto>> GetListAsync(int limit, int offset, CancellationToken cancellationToken);
    }
}
