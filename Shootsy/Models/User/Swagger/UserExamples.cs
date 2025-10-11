using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.User
{
    public class SignInRequestExample : IExamplesProvider<SignInRequest>
    {
        public SignInRequest GetExamples() => new SignInRequest
        {
            Login = "test",
            Password = "pass123"
        };
    }

    public class SignInResponseExample : IExamplesProvider<SignInResponse>
    {
        public SignInResponse GetExamples() => new SignInResponse
        {
            IdUser = 7724,
            Session = Guid.NewGuid()
        };
    }

    public class SignUpRequestExample : IExamplesProvider<SignUpRequest>
    {
        public SignUpRequest GetExamples() => new SignUpRequest
        {
            Login = "stalker",
            Password = "coolboy",
            Contact = "Аська UIN 7856",
            Firstname = "Олег",
            Lastname = "Пронин",
            Patronymic = "Двиглов",
            Discription = "Это мое описание 54346",
            Gender = 1,
            City = 2,
            Type = 1
        };
    }

    public class GetUserByIdRequestExample : IExamplesProvider<GetUserByIdRequest>
    {
        public GetUserByIdRequest GetExamples() => new GetUserByIdRequest
        {
            IdUser = 666
        };
    }

    public class GetUserByIdResponseExample : IExamplesProvider<GetUserByIdResponse>
    {
        public GetUserByIdResponse GetExamples() => new GetUserByIdResponse
        {
            Id = 666,
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            Login = "stalker",
            Gender = "Мужчина",
            City = "Новосибирск",
            Type = "Фотограф",
            Firstname = "Олег",
            Lastname = "Пронин",
            Patronymic = "Двиглов",
            Contact = "Аська UIN 7856",
            Discription = "Это мое описание 54346",
            Avatar = [byte.MinValue, 3]
        };
    }

    public class GetUsersResponseExample : IExamplesProvider<IEnumerable<GetUserByIdResponse>>
    {
        public IEnumerable<GetUserByIdResponse> GetExamples() => new[]
        {
        new GetUserByIdResponse { Id = 74, CreateDate = DateTime.UtcNow.AddDays(-3), EditDate = DateTime.UtcNow.AddDays(-2), Login = "stalkerNoob228", Gender = "Мужчина", City = "Новосибирск", Firstname = "Олег", Type = "Фотограф" },
        new GetUserByIdResponse { Id = 762, CreateDate = DateTime.UtcNow.AddDays(-67), EditDate = DateTime.UtcNow.AddDays(-6), Login = "Chelkastij", Gender = "Женщина", City = "Москва", Firstname = "Оля", Type = "Модель" },
        new GetUserByIdResponse { Id = 978, CreateDate = DateTime.UtcNow.AddDays(-6), EditDate = DateTime.UtcNow.AddDays(-2), Login = "Shabra", Gender = "Мужчина", City = "Барнаул", Firstname = "Гена", Type = "Фотограф" },
        new GetUserByIdResponse { Id = 123, CreateDate = DateTime.UtcNow.AddDays(-27), EditDate = DateTime.UtcNow.AddDays(-12), Login = "Kitaez", Gender = "Женщина", City = "Новосибирск", Firstname = "Вика", Type = "Фотограф" },
        new GetUserByIdResponse { Id = 1, CreateDate = DateTime.UtcNow.AddDays(-99), EditDate = DateTime.UtcNow.AddDays(-76), Login = "Biliboba28", Gender = "Мужчина", City = "Москва", Firstname = "Анжела", Type = "Модель" },
        };
    }

    public class UpdateUserRequestExample : IExamplesProvider<UpdateUserRequest>
    {
        public UpdateUserRequest GetExamples() => new UpdateUserRequest
        {
            IdUser = 1,
            PatchDocument = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Dtos.UserDto>()
        };
    }

    public class GetUsersRequestExample : IExamplesProvider<GetUsersRequest>
    {
        public GetUsersRequest GetExamples() => new GetUsersRequest
        {
            Offset = 5,
            Limit = 100,
            Filter = "id > 0",
            Sort = "id desc"
        };
    }
}