using Newtonsoft.Json.Linq;
using Shootsy.Database.Mongo;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.File.Swagger
{
    public class DeleteUserFilesRequestExampleModel : IExamplesProvider<DeleteUserFilesRequestModel>
    {
        public DeleteUserFilesRequestModel GetExamples() => new DeleteUserFilesRequestModel
        {
            IdUser = 543
        };
    }

    public class GetFileByIdRequestExampleModel : IExamplesProvider<GetFileByIdRequestModel>
    {
        public GetFileByIdRequestModel GetExamples() => new GetFileByIdRequestModel
        {
            IdFile = Guid.NewGuid().ToString().Replace("-", "")
        };
    }

    public class GetFileByIdResponseExampleModel : IExamplesProvider<GetFileByIdResponseModel>
    {
        public GetFileByIdResponseModel GetExamples() => new GetFileByIdResponseModel
        {
            Id = Guid.NewGuid().ToString().Replace("-", ""),
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            IdUser = 57254,
            FileInfo = new Database.Mongo.FileInfo()
            {
                FileName = "jDPlUgZS8So",
                Extension = ".jpg",
                ObjectKey = "1/97279b7eb30141469accb05116b901da/jDPlUgZS8So.jpg",
                ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%2FjDPlUgZS8So.jpg"
            }
        };
    }

    public class GetFileListRequestExampleModel : IExamplesProvider<FileStorageFilterModel>
    {
        public FileStorageFilterModel GetExamples() => new FileStorageFilterModel
        {
            Offset = 5,
            Limit = 100,
            CreatedDateFrom = DateTime.Now,
            EditDateFrom = DateTime.Now,
            CreatedDateTo = DateTime.Now,
            EditDateTo = DateTime.Now,
            FileIds = [Guid.NewGuid().ToString().Replace("-", ""), Guid.NewGuid().ToString().Replace("-", "")],
            SortBy = Enums.FileSortByEnum.CreateDate,
            SortDescending = true,
            Search = ""
        };
    }

    public class GetFileListResponseExampleModel : IExamplesProvider<IEnumerable<GetFileByIdResponseModel>>
    {
        public IEnumerable<GetFileByIdResponseModel> GetExamples() => new[]
        {
            new GetFileByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-2), EditDate = DateTime.Now, IdUser = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example1", Extension = ".jpg", ObjectKey = "1/97279b7eb30141469accb05116b901da/example1.jpg", ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%example1.jpg"} },
            new GetFileByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-3), EditDate = DateTime.Now, IdUser = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example2", Extension = ".jpg", ObjectKey = "1/97279b7eb30141469accb05116b901da/example2.jpg", ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%example2.jpg"} },
            new GetFileByIdResponseModel { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-4), EditDate = DateTime.Now, IdUser = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example3", Extension = ".jpg", ObjectKey = "1/97279b7eb30141469accb05116b901da/example3.jpg", ContentPath = "http://localhost:9000/shootsy/1%2F97279b7eb30141469accb05116b901da%example3.jpg"} },
        };
    }
}
