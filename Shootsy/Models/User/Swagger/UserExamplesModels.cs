using Shootsy.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.User
{
    public class SignInRequestExampleModel : IExamplesProvider<SignInRequestModel>
    {
        public SignInRequestModel GetExamples() => new SignInRequestModel
        {
            Login = "test",
            Password = "pass123"
        };
    }

    public class SignUpResponseExampleModel : IExamplesProvider<SignUpResponseModel>
    {
        public SignUpResponseModel GetExamples() => new SignUpResponseModel
        {
            IdUser = 7724,
            Session = Guid.NewGuid()
        };
    }

    public class SignUpRequestExampleModel : IExamplesProvider<SignUpRequestModel>
    {
        public SignUpRequestModel GetExamples() => new SignUpRequestModel
        {
            Login = "stalker",
            Password = "coolboy",
            Firstname = "Олег",
            Lastname = "Пронин",
            Discription = "Это мое описание 54346",
            Gender = 1,
            City = 2,
            ITProfession = 1
        };
    }

    public class GetUserByIdRequestExampleModel : IExamplesProvider<GetUserByIdRequestModel>
    {
        public GetUserByIdRequestModel GetExamples() => new GetUserByIdRequestModel
        {
            IdUser = 666
        };
    }

    public class GetUserByIdResponseExampleModel : IExamplesProvider<GetUserByIdResponse>
    {
        public GetUserByIdResponse GetExamples() => new GetUserByIdResponse
        {
            Id = 666,
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            Login = "stalker",
            Gender = "Мужчина",
            City = "Новосибирск",
            ITProfession = ITProfessionEnums.QA.ToString(),
            Firstname = "Олег",
            Lastname = "Пронин",
            Discription = "Это мое описание 54346"
        };
    }

    public class GetUsersResponseExampleModel : IExamplesProvider<IEnumerable<GetUserByIdResponse>>
    {
        public IEnumerable<GetUserByIdResponse> GetExamples() => new[]
        {
        new GetUserByIdResponse { Id = 74, CreateDate = DateTime.UtcNow.AddDays(-3), EditDate = DateTime.UtcNow.AddDays(-2), Login = "stalkerNoob228", Gender = "Мужчина", City = "Новосибирск", Firstname = "Олег", ITProfession = ITProfessionEnums.AQA.ToString() },
        new GetUserByIdResponse { Id = 762, CreateDate = DateTime.UtcNow.AddDays(-67), EditDate = DateTime.UtcNow.AddDays(-6), Login = "Chelkastij", Gender = "Женщина", City = "Москва", Firstname = "Оля", ITProfession = ITProfessionEnums.AccountManager.ToString() },
        new GetUserByIdResponse { Id = 978, CreateDate = DateTime.UtcNow.AddDays(-6), EditDate = DateTime.UtcNow.AddDays(-2), Login = "Shabra", Gender = "Мужчина", City = "Барнаул", Firstname = "Гена", ITProfession = ITProfessionEnums.BIAnalyst.ToString() },
        new GetUserByIdResponse { Id = 123, CreateDate = DateTime.UtcNow.AddDays(-27), EditDate = DateTime.UtcNow.AddDays(-12), Login = "Kitaez", Gender = "Женщина", City = "Новосибирск", Firstname = "Вика", ITProfession = ITProfessionEnums.CloudEngineer.ToString() },
        new GetUserByIdResponse { Id = 1, CreateDate = DateTime.UtcNow.AddDays(-99), EditDate = DateTime.UtcNow.AddDays(-76), Login = "Biliboba28", Gender = "Мужчина", City = "Москва", Firstname = "Анжела", ITProfession = ITProfessionEnums.DevOps.ToString() },
        };
    }

    public class UpdateUserRequestExampleModel : IExamplesProvider<UpdateUserRequestModel>
    {
        public UpdateUserRequestModel GetExamples() => new UpdateUserRequestModel
        {
            IdUser = 1,
            PatchDocument = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Dtos.UserDto>()
        };
    }

    public class GetUsersRequestExampleModel : IExamplesProvider<GetUsersRequestModel>
    {
        public GetUsersRequestModel GetExamples() => new GetUsersRequestModel
        {
            Offset = 5,
            Limit = 100,
            Filter = "id > 0",
            Sort = "id desc"
        };
    }
}