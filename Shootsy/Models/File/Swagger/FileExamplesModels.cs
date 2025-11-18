using Newtonsoft.Json.Linq;
using Shootsy.Database.Mongo;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.File.Swagger
{
    public class GetFileByIdResponseExampleModel : IExamplesProvider<GetFileByIdResponseModel>
    {
        public GetFileByIdResponseModel GetExamples() => new GetFileByIdResponseModel
        {
            Id = Guid.NewGuid().ToString().Replace("-", ""),
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            UserId = 57254,
            FileInfo = new Database.Mongo.FileInfo()
            {
                FileName = "jDPlUgZS8So",
                Extension = ".jpg",
                ObjectKey = "1/97279b7eb30141469accb05116b901da/jDPlUgZS8So.jpg",
                ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%2FjDPlUgZS8So.jpg"
            }
        };
    }

    public class GetFileListResponseExampleModel : IExamplesProvider<IEnumerable<GetFileByIdResponseModel>>
    {
        public IEnumerable<GetFileByIdResponseModel> GetExamples() => new[]
        {
            new GetFileByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-2), EditDate = DateTime.Now, UserId = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example1", Extension = ".jpg", ObjectKey = "1/97279b7eb30141469accb05116b901da/example1.jpg", ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%example1.jpg"} },
            new GetFileByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-3), EditDate = DateTime.Now, UserId = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example2", Extension = ".jpg", ObjectKey = "1/97279b7eb30141469accb05116b901da/example2.jpg", ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%example2.jpg"} },
            new GetFileByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-4), EditDate = DateTime.Now, UserId = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example3", Extension = ".jpg", ObjectKey = "1/97279b7eb30141469accb05116b901da/example3.jpg", ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%example3.jpg"} },
        };
    }
}