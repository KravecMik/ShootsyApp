using Shootsy.Database.Mongo;

namespace Shootsy.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<string> CreateAsync(PostEntity post, CancellationToken cancellationToken);
        Task<PostEntity>? GetByIdAsync(string idPost, CancellationToken cancellationToken);
        Task<(IReadOnlyList<PostEntity>, long total)>? GetListAsync(PostFilterModel f, CancellationToken cancellationToken);
        Task<bool> ReplaceAsync(PostEntity entity, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
    }
}