using Microsoft.AspNetCore.JsonPatch;
using Shootsy.Dtos;

namespace Shootsy.Repositories
{
    public interface IFileRepository
    {
        Task<int> CreateAsync(FileDto file, CancellationToken cancellationToken);
        Task<FileDto>? GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IReadOnlyList<FileDto>>? GetListAsync(int limit, int offset, CancellationToken cancellationToken);
        Task UpdateAsync(FileDto fileDto, JsonPatchDocument<FileDto> jsonPatchDocument, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
    }
}
