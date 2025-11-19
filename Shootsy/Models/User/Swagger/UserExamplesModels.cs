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
            Profession = ProfessionEnums.QA.ToString(),
            Category = "Quality Assurance",
            Firstname = "Олег",
            Lastname = "Пронин",
            Description = "Это мое описание 54346"
        };
    }

    public class GetUsersResponseExampleModel : IExamplesProvider<PagedResponse<UserDto>>
    {
        public PagedResponse<UserDto> GetExamples() => new PagedResponse<UserDto>
        {
            Data = new [] {
                new UserDto { Id = 74, CreateDate = DateTime.UtcNow.AddDays(-78), EditDate = DateTime.UtcNow.AddDays(-50), Login = "stalkerNoob228", Gender = "Мужчина", City = "Новосибирск", Firstname = "Олег", Lastname = "Прокин", Profession = ProfessionEnums.DevOps.ToString(), Category = "DevOps & Infrastructure" }, 
                new UserDto { Id = 1, CreateDate = DateTime.UtcNow.AddDays(-4), EditDate = DateTime.UtcNow.AddDays(-2), Login = "angelaSuper228", Gender = "Женщина", City = "Новосибирск", Firstname = "Настя", Lastname = "Шикарная", Profession = ProfessionEnums.AQA.ToString(), Category = "Quality Assurance" }, 
                new UserDto { Id = 294, CreateDate = DateTime.UtcNow.AddDays(-1), EditDate = DateTime.UtcNow.AddDays(-1), Login = "fartlover76", Gender = "Мужчина", City = "Новосибирск", Firstname = "Виктор", Lastname = "Ухов", Profession = ProfessionEnums.GraphicDesigner.ToString(), Category = "Design", Description = "Люблю рисовать каки" } 
            },
            Page = 1, 
            PageSize = 20, 
            TotalCount = 200,
            TotalPages = 10 
        };
    }
}