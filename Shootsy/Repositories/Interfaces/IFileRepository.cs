using Shootsy.Database.Mongo;

namespace Shootsy.Repositories
{
    public interface IFileRepository
    {
        Task<string> CreateFileAsync(FileStorageEntity file, CancellationToken cancellationToken);
        Task<FileStorageEntity?> GetFileByIdAsync(string idFile, CancellationToken cancellationToken);
        Task<IReadOnlyList<FileStorageEntity?>> GetFilesListByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<bool> UpdateFileAsync(FileStorageEntity entity, CancellationToken cancellationToken);
        Task<bool> DeleteFileByIdAsync(string id, CancellationToken cancellationToken);
    }
}