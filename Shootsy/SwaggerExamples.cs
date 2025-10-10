using Shootsy.Models;
using Shootsy.Models.File;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy
{
    public class GetFileByIdRequestExample : IExamplesProvider<GetFileByIdModel>
    {
        public GetFileByIdModel GetExamples() => new GetFileByIdModel
        {
            Id = 123,
            Session = "abcdef123456"
        };
    }

    public class FileModelResponseExample : IExamplesProvider<FileModelResponse>
    {
        public FileModelResponse GetExamples() => new FileModelResponse
        {
            Id = 123,
            CreateDate = DateTime.UtcNow.AddDays(-2),
            EditDate = DateTime.UtcNow,
            UserID = 42,
            Extension = "jpg",
            ContentPath = "/content/files/123.jpg"
        };
    }

    public class GetFilesModelRequestExample : IExamplesProvider<GetFilesModel>
    {
        public GetFilesModel GetExamples() => new GetFilesModel
        {
            Offset = 1,
            Limit = 2,
            Filter = "id > 0",
            Sort = "id desc"
        };
    }

    public class GetFilesResponseExample : IExamplesProvider<IEnumerable<FileModelResponse>>
    {
        public IEnumerable<FileModelResponse> GetExamples() => new[]
        {
        new FileModelResponse { Id = 1, CreateDate = DateTime.UtcNow.AddDays(-3), EditDate = DateTime.UtcNow.AddDays(-2), UserID = 13, Extension = "png", ContentPath = "/content/files/1.png" },
        new FileModelResponse { Id = 2, CreateDate = DateTime.UtcNow.AddDays(-1), EditDate = DateTime.UtcNow, UserID = 12, Extension = "jpg", ContentPath = "/content/files/2.jpg" }
    };
    }
}
