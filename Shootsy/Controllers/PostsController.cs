using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shootsy.Database.Mongo;
using Shootsy.Models.Post;
using Shootsy.Models.Post.Swagger;
using Shootsy.Repositories;
using Shootsy.Repositories.Interfaces;
using Shootsy.Service;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Controllers
{
    [ApiController]
    [Route("Posts")]
    public class PostsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public PostsController(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Создать публикацию пользователя")]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(CreatePostRequestModel), typeof(CreatePostRequestExampleModel))]
        [SwaggerResponse(statusCode: 201, description: "Created", type: typeof(string))]
        public async Task<IActionResult> CreatePostAsync([FromBody, BindRequired] CreatePostRequestModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (user is null) return NotFound();

            var postId = await _postRepository.CreateAsync(new PostEntity
            {
                CreateDate = DateTime.UtcNow,
                EditDate = DateTime.UtcNow,
                IdUser = model.IdUser,
                Text = model.Text

            }, cancellationToken);

            return CreatedAtAction(nameof(CreatePostAsync), postId);
        }

        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получение карточки публикации по идентификатору")]
        [SwaggerRequestExample(typeof(GetPostByIdRequestModel), typeof(GetPostByIdRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(GetPostByIdResponseModel))]
        [SwaggerResponseExample(200, typeof(GetPostByIdResponseExampleModel))]
        public async Task<IActionResult> GetPostByIdAsync([FromQuery] GetPostByIdRequestModel model, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetByIdAsync(model.IdPost, cancellationToken);
            if (post is null) return NotFound();
            return Ok(post);
        }

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список карточек публикаций")]
        [SwaggerRequestExample(typeof(PostFilterModel), typeof(GetPostListRequestExampleModel))]
        [SwaggerResponse(statusCode: 200, description: "OK", type: typeof(IEnumerable<GetPostByIdResponseModel>))]
        [SwaggerResponseExample(200, typeof(GetPostListResponseExampleModel))]
        public async Task<IActionResult> GetPostsAsync([FromQuery] PostFilterModel filter, CancellationToken cancellationToken = default)
        {
            var result = await _postRepository.GetListAsync(filter, cancellationToken);
            return Ok(result.Item1);
        }

        [Authorize]
        [HttpPatch("{id}")]
        [Consumes("application/json-patch+json")]
        [SwaggerOperation(Summary = "Обновить карточку публикации")]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute(Name = "id")] string id, [FromBody] JsonPatchDocument<PostEntity> patch, CancellationToken cancellationToken = default)
        {
            var entity = await _postRepository.GetByIdAsync(id, cancellationToken);
            if (entity is null) return NotFound();

            var targetPath = patch.Operations.Select(x => x).ToList();

            foreach (var item in targetPath)
            {
                if (item.path.ToLower().Contains("iduser"))
                {
                    var user = await _userRepository.GetByIdAsync(int.Parse(item.value.ToString()), cancellationToken);
                    if (user is null) return NotFound();
                }
                if (item.path.ToLower().Contains("iduser"))
                {
                    ModelState.AddModelError("iduser", "Данное поле редактировать запрещено");
                    return ValidationProblem();
                }
            }

            patch.ApplyTo(entity, ModelState);
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            entity.EditDate = DateTime.UtcNow;

            var updated = await _postRepository.ReplaceAsync(entity, cancellationToken);
            if (!updated) return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить публикацию по идентификатору")]
        [SwaggerRequestExample(typeof(GetPostByIdRequestModel), typeof(GetPostByIdRequestExampleModel))]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeletePostByIdAsync([FromQuery] GetPostByIdRequestModel model, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetByIdAsync(model.IdPost, cancellationToken);
            if (post is null) return NotFound();

            await _postRepository.DeleteByIdAsync(model.IdPost, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("iduser={id}")]
        [SwaggerOperation(Summary = "Удалить все публикации по идентификатору пользователя")]
        [SwaggerRequestExample(typeof(DeleteUserPostsRequestModel), typeof(DeleteUserPostsRequestExampleModel))]
        [SwaggerResponse(statusCode: 204, description: "NoContent")]
        public async Task<IActionResult> DeleteUserPostsAsync([FromQuery] DeleteUserPostsRequestModel model, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(model.IdUser, cancellationToken);
            if (user is null) return NotFound();

            var postList = await _postRepository.GetListAsync(new PostFilterModel { UserId = model.IdUser }, cancellationToken);

            foreach (var Post in postList.Item1)
            {
                await _postRepository.DeleteByIdAsync(Post.Id, cancellationToken);
            }
            return NoContent();
        }
    }
}