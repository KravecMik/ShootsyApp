using Newtonsoft.Json.Linq;
using Shootsy.Database.Mongo;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.File.Swagger
{
    public class CreateFileResponseExample : IExamplesProvider<CreateFileResponse>
    {
        public CreateFileResponse GetExamples() => new CreateFileResponse
        {
            IdFile = Guid.NewGuid().ToString().Replace("-", "")
        };
    }

    public class DeleteUserFilesRequestExample : IExamplesProvider<DeleteUserFilesRequest>
    {
        public DeleteUserFilesRequest GetExamples() => new DeleteUserFilesRequest
        {
            IdUser = 543
        };
    }

    public class GetFileByIdRequestExample : IExamplesProvider<GetFileByIdRequest>
    {
        public GetFileByIdRequest GetExamples() => new GetFileByIdRequest
        {
            IdFile = Guid.NewGuid().ToString().Replace("-", "")
        };
    }

    public class GetFileByIdResponseExample : IExamplesProvider<GetFileByIdResponse>
    {
        public GetFileByIdResponse GetExamples() => new GetFileByIdResponse
        {
            Id = Guid.NewGuid().ToString().Replace("-", ""),
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            IdUser = 57254,
            FileInfo = new Database.Mongo.FileInfo()
            {
                FileName = "example",
                Extension = "jpg",
                ContentPath = "minio/gdfsjsfadfgy/example.jpg"
            }
        };
    }

    public class GetFileListRequestExample : IExamplesProvider<FileStorageFilter>
    {
        public FileStorageFilter GetExamples() => new FileStorageFilter
        {
            Offset = 5,
            Limit = 100,
            CreatedDateFrom = DateTime.Now,
            EditDateFrom = DateTime.Now,
            CreatedDateTo = DateTime.Now,
            EditDateTo = DateTime.Now,
            FileIds = [Guid.NewGuid().ToString().Replace("-", ""), Guid.NewGuid().ToString().Replace("-", "")],
            SortBy = Enums.SortByEnum.CreateDate,
            SortDescending = true,
            Search = ""
        };
    }

    public class GetFileListResponseExample : IExamplesProvider<IEnumerable<GetFileByIdResponse>>
    {
        public IEnumerable<GetFileByIdResponse> GetExamples() => new[]
        {
            new GetFileByIdResponse { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-2), EditDate = DateTime.Now, IdUser = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example1", Extension = "jpg", ContentPath = "minio/gdfsjsfadfgy/example1.jpg"} },
            new GetFileByIdResponse { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-3), EditDate = DateTime.Now, IdUser = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example2", Extension = "jpg", ContentPath = "minio/gdfsjsfadfgy/example2.jpg"} },
            new GetFileByIdResponse { Id = Guid.NewGuid().ToString().Replace("-", ""), CreateDate = DateTime.Now.AddDays(-4), EditDate = DateTime.Now, IdUser = 57954, FileInfo = new Database.Mongo.FileInfo {FileName = "example3", Extension = "jpg", ContentPath = "minio/gdfsjsfadfgy/example3.jpg"} },
        };
    }
}
