using Microsoft.AspNetCore.JsonPatch;
using Shootsy.Database.Mongo;

namespace Shootsy.Repositories
{
    public interface IFileRepository
    {
        Task<string> CreateAsync(FileStorageEntity file, CancellationToken cancellationToken);
        Task<FileStorageEntity>? GetByIdAsync(string idFile, CancellationToken cancellationToken);
        Task<(IReadOnlyList<FileStorageEntity>, long total)>? GetListAsync(FileStorageFilterModel f, CancellationToken cancellationToken);
        Task<bool> ReplaceAsync(FileStorageEntity entity, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
    }
}
