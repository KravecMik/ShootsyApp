using Shootsy.Models.Dtos;
using Shootsy.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Shootsy.Models.User
{
    public class SignInRequestExampleModel : IExamplesProvider<SignInRequestModel>
    {
        public SignInRequestModel GetExamples() => new SignInRequestModel
        {
            Login = "123",
            Password = "123123123"
        };
    }

    public class SignUpResponseExampleModel : IExamplesProvider<SignUpResponseModel>
    {
        public SignUpResponseModel GetExamples() => new SignUpResponseModel
        {
            UserId = 7724,
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
            Description = "Это мое описание 54346",
            Gender = 1,
            City = 2,
            ITProfession = 1
        };
    }

    public class GetUserByIdResponseExampleModel : IExamplesProvider<UserDto>
    {
        public UserDto GetExamples() => new UserDto
        {
            Id = 666,
            CreateDate = DateTime.Now.AddDays(-1),
            EditDate = DateTime.Now,
            Login = "stalker",
            Gender = "Мужчина",
            City = "Новосибирск",
            Profession = ITProfessionEnums.QA.ToString(),
            Category = "Quality Assurance",
            Firstname = "Олег",
            Lastname = "Пронин",
            Description = "Это мое описание 54346"
        };
    }

    public class GetUsersResponseExampleModel : IExamplesProvider<IEnumerable<UserDto>>
    {
        public IEnumerable<UserDto> GetExamples() => new[]
        {
        new UserDto { Id = 74, CreateDate = DateTime.UtcNow.AddDays(-3), EditDate = DateTime.UtcNow.AddDays(-2), Login = "stalkerNoob228", Gender = "Мужчина", City = "Новосибирск", Firstname = "Олег", Lastname = "Прокин", Profession = ITProfessionEnums.AQA.ToString(), Category = "Quality Assurance"},
        new UserDto { Id = 762, CreateDate = DateTime.UtcNow.AddDays(-67), EditDate = DateTime.UtcNow.AddDays(-6), Login = "Chelkastij", Gender = "Женщина", City = "Москва", Firstname = "Оля", Profession = ITProfessionEnums.AndroidDeveloper.ToString(), Category = "Development"},
        new UserDto { Id = 978, CreateDate = DateTime.UtcNow.AddDays(-6), EditDate = DateTime.UtcNow.AddDays(-2), Login = "Shabra", Gender = "Мужчина", City = "Барнаул", Firstname = "Гена", Description = "Люблю шарпы и маму", Profession = ITProfessionEnums.BIAnalyst.ToString(), Category = "Data"},
        new UserDto { Id = 123, CreateDate = DateTime.UtcNow.AddDays(-27), EditDate = DateTime.UtcNow.AddDays(-12), Login = "Kitaez", Gender = "Женщина", City = "Новосибирск", Firstname = "Вика", Profession = ITProfessionEnums.CloudEngineer.ToString(), Category = "DevOps & Infrastructure"},
        new UserDto { Id = 1, CreateDate = DateTime.UtcNow.AddDays(-99), EditDate = DateTime.UtcNow.AddDays(-76), Login = "Biliboba28", Gender = "Мужчина", City = "Москва", Firstname = "Анжела", Profession = ITProfessionEnums.DevOps.ToString(), Category = "DevOps & Infrastructure"}
        };
    }
}