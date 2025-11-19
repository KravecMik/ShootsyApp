using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Shootsy.Database.Entities;
using Shootsy.Models;
using Shootsy.Models.Dtos;
using Shootsy.Models.Post;
using Shootsy.Models.Post.Swagger;
using Shootsy.Repositories;
using Shootsy.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Posts")]
    public class PostsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public PostsController(IUserRepository userRepository, IPostRepository postRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Создать публикацию пользователя")]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(CreatePostRequestModel), typeof(CreatePostRequestExampleModel))]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(int))]
        public async Task<IActionResult> CreatePostAsync([FromBody, BindRequired] CreatePostRequestModel request, CancellationToken cancellationToken = default)
        {
            var userId = await GetCurrentUserIdAsync(_userRepository, cancellationToken);
            var post = new PostEntity
            {
                UserId = userId.Id,
                Text = request.Text
            };

            var postId = await _postRepository.CreatePostAsync(post, cancellationToken);
            return Created(string.Empty, new { postId });
        }

        [Authorize]
        [HttpGet("{postId:int}")]
        [SwaggerOperation(Summary = "Получение карточки публикации по идентификатору")]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(PostDto))]
        [SwaggerResponseExample(200, typeof(GetPostByIdResponseExampleModel))]
        public async Task<IActionResult> GetPostByIdAsync([FromRoute] int postId, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetPostByIdAsync(postId, cancellationToken);
            if (post is null)
                return NotFound();

            var result = _mapper.Map<PostDto>(post);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список карточек публикаций")]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(IEnumerable<PostDto>))]
        [SwaggerResponseExample(200, typeof(GetPostListResponseExampleModel))]
        public async Task<IActionResult> GetPostsAsync([FromQuery] PostFilterDto filter, CancellationToken cancellationToken = default)
        {
            var (postsEntities, totalCount) = await _postRepository.GetPostsListAsync(filter, cancellationToken);
            var posts = _mapper.Map<IReadOnlyList<PostDto>>(postsEntities);

            var response = new PagedResponse<PostDto>
            {
                Data = posts,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPatch("{postId:int}")]
        [Consumes("application/json-patch+json")]
        [SwaggerOperation(Summary = "Обновить карточку публикации")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] int postId, [FromBody] JsonPatchDocument<PostEntity> patch, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetPostByIdAsync(postId, cancellationToken);
            if (post is null)
                return NotFound("Публикация по указанному идентификатору не найдена");

            var (hasAccess, errorResult) = await CheckAccessByUserIdAsync(post.UserId, _userRepository, cancellationToken);
            if (!hasAccess)
            {
                return errorResult!;
            }

            foreach (var operation in patch.Operations)
            {
                if (operation.path.Equals("/UserId", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("UserId", "Поле недоступно для редактирования");
                    return ValidationProblem(ModelState);
                }
            }

            await _postRepository.UpdatePostAsync(post, patch, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{postId:int}")]
        [SwaggerOperation(Summary = "Удалить публикацию по идентификатору")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeletePostByIdAsync([FromRoute] int postId, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetPostByIdAsync(postId, cancellationToken);
            if (post is null)
                return NotFound("Публикация по указанному идентификатору не найдена");

            var (hasAccess, errorResult) = await CheckAccessByUserIdAsync(post.UserId, _userRepository, cancellationToken);
            if (!hasAccess)
            {
                return errorResult!;
            }

            await _postRepository.DeletePostByIdAsync(postId, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{postId:int}/comments")]
        [SwaggerOperation(Summary = "Добавить комментарий к публикации")]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(int))]
        public async Task<IActionResult> CreateCommentAsync([FromRoute] int postId, [FromBody] AddCommentRequestModel request, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetPostByIdAsync(postId, cancellationToken);
            if (post is null)
                return NotFound("Публикация не найдена");

            var userId = await GetCurrentUserIdAsync(_userRepository, cancellationToken);
            var comment = new CommentEntity
            {
                Text = request.Text,
                UserId = userId.Id,
                PostId = postId
            };

            var commentId = await _postRepository.CreateCommentAsync(comment, cancellationToken);
            return Created(string.Empty, new { commentId });
        }

        [Authorize]
        [HttpGet("{postId:int}/comments")]
        [SwaggerOperation(Summary = "Получить все комментарии к посту по идентификатору")]
        [SwaggerResponse(statusCode: 200, description: "OK")]
        public async Task<IActionResult> GetCommentsByPostIdAsync([FromRoute] int postId, CancellationToken cancellationToken = default)
        {
            var commentList = await _postRepository.GetCommentListAsync(postId, cancellationToken);
            var result = _mapper.Map<IReadOnlyList<CommentDto>>(commentList);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{postId:int}/comments/{commentId:int}")]
        [SwaggerOperation(Summary = "Получить комментарий по идентификатору")]
        [SwaggerResponse(statusCode: 200, description: "OK")]
        public async Task<IActionResult> GetCommentByIdAsync([FromRoute] int postId, [FromRoute] int commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _postRepository.GetCommentByIdAsync(commentId, cancellationToken);
            if (comment is null || comment.PostId != postId)
                return NotFound("Комментарий не найден");

            var result = _mapper.Map<CommentDto>(comment);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("{postId:int}/comments/{commentId:int}")]
        [SwaggerOperation(Summary = "Добавить комментарий на другой комментарий к публикации")]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(int))]
        public async Task<IActionResult> CreateCommentReplyAsync([FromRoute] int postId, [FromRoute] int commentId, [FromBody] AddCommentRequestModel request, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetPostByIdAsync(postId, cancellationToken);
            if (post is null)
                return NotFound("Публикация не найдена");

            var userId = await GetCurrentUserIdAsync(_userRepository, cancellationToken);
            var comment = new CommentEntity
            {
                Text = request.Text,
                UserId = userId.Id,
                PostId = postId,
                ParentCommentId = commentId
            };

            var answerCommentId = await _postRepository.CreateCommentAsync(comment, cancellationToken);
            return Created(string.Empty, new { answerCommentId });
        }

        [Authorize]
        [HttpPatch("{postId:int}/comments/{commentId:int}")]
        [Consumes("application/json-patch+json")]
        [SwaggerOperation(Summary = "Обновить комментарий")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int postId, [FromRoute] int commentId, [FromBody] JsonPatchDocument<CommentEntity> patch, CancellationToken cancellationToken = default)
        {
            var comment = await _postRepository.GetCommentByIdAsync(commentId, cancellationToken);
            if (comment is null || comment.PostId != postId)
                return NotFound("Комментарий не найден");

            var (hasAccess, errorResult) = await CheckAccessByUserIdAsync(comment.UserId, _userRepository, cancellationToken);
            if (!hasAccess)
            {
                return errorResult!;
            }

            foreach (var operation in patch.Operations)
            {
                if (operation.path.Equals("/UserId", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("UserId", "Изменение автора комментария запрещено");
                    return ValidationProblem(ModelState);
                }

                if (operation.path.Equals("/PostId", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("PostId", "Изменение публикации комментария запрещено");
                    return ValidationProblem(ModelState);
                }

                if (operation.path.Equals("/ParentCommentId", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("ParentCommentId", "Изменение родительского комментария запрещено");
                    return ValidationProblem(ModelState);
                }
            }

            patch.ApplyTo(comment, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            comment.EditDate = DateTime.UtcNow;

            var updated = await _postRepository.UpdateCommentAsync(comment, cancellationToken);
            if (!updated)
                return StatusCode(500, "Не удалось обновить комментарий");

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{postId:int}/comments/{commentId:int}")]
        [SwaggerOperation(Summary = "Удалить комментарий")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteCommentAsync([FromRoute] int postId, [FromRoute] int commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _postRepository.GetCommentByIdAsync(commentId, cancellationToken);
            if (comment is null || comment.PostId != postId)
                return NotFound("Комментарий не найден");

            var (hasAccess, errorResult) = await CheckAccessByUserIdAsync(comment.UserId, _userRepository, cancellationToken);
            if (!hasAccess)
            {
                return errorResult!;
            }

            var deleted = await _postRepository.DeleteCommentAsync(commentId, cancellationToken);
            if (!deleted)
                return StatusCode(500, "Не удалось удалить комментарий");

            return NoContent();
        }

        [Authorize]
        [HttpPost("{postId:int}/likes")]
        [SwaggerOperation(Summary = "Добавить лайк к публикации")]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(int))]
        public async Task<IActionResult> CreatePostLikeAsync([FromRoute] int postId, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetPostByIdAsync(postId, cancellationToken);
            if (post is null)
                return NotFound("Публикация не найдена");

            var userId = await GetCurrentUserIdAsync(_userRepository, cancellationToken);

            var like = new LikeEntity
            {
                UserId = userId.Id,
                PostId = postId
            };

            var added = await _postRepository.CreateLikeAsync(like, cancellationToken);
            if (!added)
            {
                return BadRequest("Лайк уже был добавлен данным пользователем");
            }

            return Created(string.Empty, new { like.Id });
        }

        [Authorize]
        [HttpPost("{postId:int}/comments/{commentId:int}/likes")]
        [SwaggerOperation(Summary = "Добавить лайк к комментарию")]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(int))]
        public async Task<IActionResult> CreateCommentLikeAsync([FromRoute] int postId, [FromRoute] int commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _postRepository.GetCommentByIdAsync(commentId, cancellationToken);
            if (comment is null || comment.PostId != postId)
                return NotFound("Комментарий не найден");

            var userId = await GetCurrentUserIdAsync(_userRepository, cancellationToken);

            var like = new LikeEntity
            {
                UserId = userId.Id,
                CommentId = commentId
            };

            var added = await _postRepository.CreateLikeAsync(like, cancellationToken);
            if (!added)
            {
                return BadRequest("Лайк уже был добавлен данным пользователем");
            }

            return Created(string.Empty, new { like.Id });
        }

        [Authorize]
        [HttpGet("{postId:int}/likes/count")]
        [SwaggerOperation(Summary = "Получить количество лайков к посту")]
        [SwaggerResponse(statusCode: 200, description: "ОК", type: typeof(int))]
        public async Task<IActionResult> GetLikeCountByPostIdAsync([FromRoute] int postId, CancellationToken cancellationToken = default)
        {
            var count = await _postRepository.GetLikeCountByPostIdAsync(postId, cancellationToken);
            return Ok(count);
        }
    }
}