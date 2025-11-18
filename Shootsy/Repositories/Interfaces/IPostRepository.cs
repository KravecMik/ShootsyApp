using Microsoft.AspNetCore.JsonPatch;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;

namespace Shootsy.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<int> CreatePostAsync(PostEntity post, CancellationToken cancellationToken);
        Task<PostEntity?> GetPostByIdAsync(int id, CancellationToken cancellationToken);
        Task<(IReadOnlyList<PostEntity> Posts, int TotalCount)> GetPostsListAsync(PostFilterDto filter, CancellationToken cancellationToken);
        Task UpdatePostAsync(PostEntity post, JsonPatchDocument<PostEntity> jsonPatchDocument, CancellationToken cancellationToken);
        Task DeletePostByIdAsync(int id, CancellationToken cancellationToken);
        Task<CommentEntity?> GetCommentByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> AddCommentAsync(CommentEntity comment, CancellationToken cancellationToken);
        Task<bool> UpdateCommentAsync(CommentEntity comment, CancellationToken cancellationToken);
        Task<bool> DeleteCommentAsync(int commentId, CancellationToken cancellationToken);
        Task<bool> AddLikeAsync(LikeEntity like, CancellationToken cancellationToken);
        Task<bool> RemoveLikeAsync(int likeId, CancellationToken cancellationToken);
        Task<bool> RemoveLikeAsync(int userId, int? postId, int? commentId, CancellationToken cancellationToken);
        Task <int> GetLikeCountByPostIdAsync(int postId, CancellationToken cancellationToken);
        Task<IReadOnlyList<CommentEntity?>> GetCommentListAsync(int postId, CancellationToken cancellationToken);
    }
}