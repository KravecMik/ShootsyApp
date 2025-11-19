using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shootsy.Database;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;
using Shootsy.Repositories.Interfaces;
using Shootsy.Service;

namespace Shootsy.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IKafkaProducerService _kafkaProducer;

        public PostRepository(ApplicationContext context, IMapper mapper, IKafkaProducerService kafkaProducer)
        {
            _context = context;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<int> CreatePostAsync(PostEntity post, CancellationToken cancellationToken)
        {
            post.CreateDate = DateTime.UtcNow;
            post.EditDate = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);
            await _kafkaProducer.ProducePostEventAsync("post.created", new { post.Id });

            return post.Id;
        }

        public async Task DeletePostByIdAsync(int postId, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.Where(x => x.Id == postId).FirstOrDefaultAsync();
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync(cancellationToken);
                await _kafkaProducer.ProducePostEventAsync("post.deleted", new { postId });
            }
        }

        public async Task<PostEntity?> GetPostByIdAsync(int postId, CancellationToken cancellationToken)
        {
            return await _context.Posts.AsNoTracking()
                .Where(x => x.Id == postId)
                .FirstOrDefaultAsync();
        }

        public async Task<(IReadOnlyList<PostEntity> Posts, int TotalCount)> GetPostsListAsync(PostFilterDto filter, CancellationToken cancellationToken = default)
        {
            var query = _context.Posts
                    .AsNoTracking();

            if (filter.UserId.HasValue)
                query = query.Where(p => p.UserId == filter.UserId.Value);

            if (!string.IsNullOrEmpty(filter.Search))
            {
                var searchTerm = filter.Search.ToLower();
                query = query.Where(p => p.Text.ToLower().Contains(searchTerm));
            }

            if (filter.FromDate.HasValue)
                query = query.Where(p => p.CreateDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(p => p.CreateDate <= filter.ToDate.Value);

            query = filter.SortBy?.ToLower() switch
            {
                "editdate" => filter.SortDesc ? query.OrderByDescending(p => p.EditDate) : query.OrderBy(p => p.EditDate),
                "userid" => filter.SortDesc ? query.OrderByDescending(p => p.UserId) : query.OrderBy(p => p.UserId),
                _ => filter.SortDesc ? query.OrderByDescending(p => p.CreateDate) : query.OrderBy(p => p.CreateDate)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var posts = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            return (posts, totalCount);
        }

        public async Task UpdatePostAsync(PostEntity post, JsonPatchDocument<PostEntity> jsonPatchDocument, CancellationToken cancellationToken = default)
        {
            jsonPatchDocument.ApplyTo(post);
            post.EditDate = DateTime.UtcNow;
            _context.Update(post);
            await _context.SaveChangesAsync();
            await _kafkaProducer.ProducePostEventAsync("post.updated", new { post.Id });
        }

        public async Task<CommentEntity?> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Comments.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<CommentEntity?>> GetCommentListAsync(int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Comments.AsNoTracking()
                .Where(c => c.PostId == postId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CreateCommentAsync(CommentEntity comment, CancellationToken cancellationToken = default)
        {
            comment.CreateDate = DateTime.UtcNow;
            comment.EditDate = DateTime.UtcNow;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);

            await _kafkaProducer.ProducePostEventAsync("comment.created", new { comment.Id });

            return comment.Id;
        }

        public async Task<bool> UpdateCommentAsync(CommentEntity comment, CancellationToken cancellationToken = default)
        {
            comment.EditDate = DateTime.UtcNow;

            _context.Comments.Update(comment);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await _kafkaProducer.ProducePostEventAsync("comment.updated", new { comment.Id });
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteCommentAsync(int commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments.FindAsync(new object[] { commentId }, cancellationToken);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);
                await _kafkaProducer.ProducePostEventAsync("comment.deleted", new { commentId });
                return true;
            }

            return false;
        }

        public async Task<bool> CreateLikeAsync(LikeEntity like, CancellationToken cancellationToken = default)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == like.UserId &&
                    ((like.PostId.HasValue && l.PostId == like.PostId) ||
                     (like.CommentId.HasValue && l.CommentId == like.CommentId)),
                    cancellationToken);

            if (existingLike != null)
                return false;

            like.CreateDate = DateTime.UtcNow;
            _context.Likes.Add(like);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                var targetId = like.PostId ?? like.CommentId;
                await _kafkaProducer.ProducePostEventAsync("like.added", new { like.Id, targetId });
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveLikeAsync(int likeId, CancellationToken cancellationToken = default)
        {
            var like = await _context.Likes.FindAsync(new object[] { likeId }, cancellationToken);
            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync(cancellationToken);

                var targetId = like.PostId ?? like.CommentId;
                await _kafkaProducer.ProducePostEventAsync("like.removed", new { likeId, targetId });
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveLikeAsync(int userId, int? postId, int? commentId, CancellationToken cancellationToken = default)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId &&
                    l.PostId == postId && l.CommentId == commentId,
                    cancellationToken);

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync(cancellationToken);

                var targetId = postId ?? commentId;
                await _kafkaProducer.ProducePostEventAsync("like.removed", new { like.Id, targetId });
                return true;
            }

            return false;
        }

        public async Task<int> GetLikeCountByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Likes.AsNoTracking()
                .Where(c => c.PostId == postId)
                .CountAsync(cancellationToken);
        }
    }
}