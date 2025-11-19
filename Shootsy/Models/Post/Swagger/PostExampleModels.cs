using Shootsy.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.Post.Swagger
{
    public class CreatePostRequestExampleModel : IExamplesProvider<CreatePostRequestModel>
    {
        public CreatePostRequestModel GetExamples() => new CreatePostRequestModel
        {
            Text = "Этот супчик очень вкусный. Спасибо!"
        };
    }

    public class GetPostByIdResponseExampleModel : IExamplesProvider<PostDto>
    {
        public PostDto GetExamples() => new PostDto
        {
            Id = 5,
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            UserId = 57254,
            Text = "Что то там про французские булки"
        };
    }

    public class GetPostListResponseExampleModel : IExamplesProvider<PagedResponse<PostDto>>
    {
        public PagedResponse<PostDto> GetExamples() => new PagedResponse<PostDto>
        {
            Data = new[] {
                new PostDto { Id = 1, CreateDate = DateTime.Now.AddDays(-2), EditDate = DateTime.Now, UserId = 57954, Text = "бла бла бла" },
                new PostDto { Id = 2, CreateDate = DateTime.Now.AddDays(-3), EditDate = DateTime.Now, UserId = 57954, Text = "я люблю манную кашу" },
                new PostDto { Id = 3, CreateDate = DateTime.Now.AddDays(-4), EditDate = DateTime.Now, UserId = 57954, Text = "дева4ки, я тащусь" },
            },
            Page = 1,
            PageSize = 20,
            TotalCount = 200,
            TotalPages = 10
        };
    }
}