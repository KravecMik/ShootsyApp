using Shootsy.Database.Mongo;
using Shootsy.Models.Post;
using Shootsy.Models.User;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.Post.Swagger
{
    public class CreatePostRequestExampleModel : IExamplesProvider<CreatePostRequestModel>
    {
        public CreatePostRequestModel GetExamples() => new CreatePostRequestModel
        {
            IdUser = 111,
            Text = "Этот супчик очень вкусный. Спасибо!"
        };
    }

    public class DeleteUserPostsRequestExampleModel : IExamplesProvider<DeleteUserPostsRequestModel>
    {
        public DeleteUserPostsRequestModel GetExamples() => new DeleteUserPostsRequestModel
        {
            IdUser = 666
        };
    }

    public class GetPostByIdRequestExampleModel : IExamplesProvider<GetPostByIdRequestModel>
    {
        public GetPostByIdRequestModel GetExamples() => new GetPostByIdRequestModel
        {
            IdPost = Guid.NewGuid().ToString().Replace("-", "")
        };
    }

    public class GetPostByIdResponseExampleModel : IExamplesProvider<GetPostByIdResponseModel>
    {
        public GetPostByIdResponseModel GetExamples() => new GetPostByIdResponseModel
        {
            Id = Guid.NewGuid().ToString().Replace("-", ""),
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            IdUser = 57254,
            Text = "Что то там про французские булки"
        };
    }

    public class GetPostListRequestExampleModel : IExamplesProvider<PostFilterModel>
    {
        public PostFilterModel GetExamples() => new PostFilterModel
        {
            Offset = 5,
            Limit = 100,
            CreatedDateFrom = DateTime.Now,
            EditDateFrom = DateTime.Now,
            CreatedDateTo = DateTime.Now,
            EditDateTo = DateTime.Now,
            PostIds = [Guid.NewGuid().ToString().Replace("-", ""), Guid.NewGuid().ToString().Replace("-", "")],
            SortBy = Enums.PostSortByEnum.CreateDate,
            SortDescending = true,
            Search = ""
        };
    }

    public class GetPostListResponseExampleModel : IExamplesProvider<IEnumerable<GetPostByIdResponseModel>>
    {
        public IEnumerable<GetPostByIdResponseModel> GetExamples() => new[]
        {
            new GetPostByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-2), EditDate = DateTime.Now, IdUser = 57954, Text = "бла бла бла" },
            new GetPostByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-3), EditDate = DateTime.Now, IdUser = 57954, Text = "я люблю манную кашу" },
            new GetPostByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-4), EditDate = DateTime.Now, IdUser = 57954, Text = "дева4ки, я тащусь" },
        };
    }
}