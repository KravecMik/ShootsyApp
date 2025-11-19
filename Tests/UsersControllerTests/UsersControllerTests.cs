using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shootsy.Controllers;
using Shootsy.Database.Entities;
using Shootsy.Enums;
using Shootsy.Models.Dtos;
using Shootsy.Models.User;
using Shootsy.Repositories;
using Shootsy.Service;

namespace Tests.UsersControllerTests;

[TestFixture]
public class UsersControllerTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IPasswordService> _mockPasswordService;
    private UsersController _controller;
    private Faker _faker;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockPasswordService = new Mock<IPasswordService>();
        _controller = new UsersController(_mockUserRepository.Object, _mockMapper.Object, _mockPasswordService.Object);
        _faker = new Faker();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Test]
    public async Task SignUpAsync_WithValidData_ReturnsCreatedResult()
    {
        var request = new SignUpRequestModel
        {
            Login = _faker.Random.String2(6),
            Password = _faker.Random.String2(7),
            Firstname = _faker.Random.String2(5),
            Lastname = _faker.Random.String2(8),
            Description = _faker.Random.String2(100),
            Gender = (int)_faker.PickRandom<GenderEnums>(),
            City = (int)_faker.PickRandom<CityEnum>(),
            Profession = (int)_faker.PickRandom<ProfessionEnums>()
        };

        var passwordService = new PasswordService();
        var hashedPassword = passwordService.EncryptString(request.Password, request.Login);

        var userEntity = new UserEntity
        {
            Id = _faker.Random.Int(1,200),
            Login = request.Login,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Description = request.Description,
            GenderId = request.Gender,
            CityId = request.City,
            ProfessionId = request.Profession,
            Password = hashedPassword
        };

        var sessionGuid = Guid.NewGuid();

        _mockMapper.Setup(m => m.Map<UserEntity>(request))
                   .Returns(userEntity);

        _mockUserRepository.Setup(r => r.GetUserByLoginAsync(request.Login, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((UserEntity)null);

        _mockUserRepository.Setup(r => r.CreateUserAsync(userEntity, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity.Id);

        _mockUserRepository.Setup(r => r.CreateSessionAsync(userEntity.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(sessionGuid);

        var result = await _controller.SignUpAsync(request, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(201));

        var response = objectResult.Value as SignUpResponseModel;
        Assert.That(response, Is.Not.Null);
        Assert.That(response.UserId, Is.EqualTo(userEntity.Id));
        Assert.That(response.Session, Is.EqualTo(sessionGuid));

        _mockUserRepository.Verify(r => r.GetUserByLoginAsync(request.Login, It.IsAny<CancellationToken>()), Times.Once, "ћетод GetUserByLoginAsync должен быть вызван ровно один раз дл€ проверки существовани€ пользовател€");
        _mockUserRepository.Verify(r => r.CreateUserAsync(userEntity, It.IsAny<CancellationToken>()), Times.Once, "ћетод CreateUserAsync должен быть вызван ровно один раз дл€ сохранени€ пользовател€ в базу данных");
        _mockUserRepository.Verify(r => r.CreateSessionAsync(userEntity.Id, It.IsAny<CancellationToken>()), Times.Once, "ћетод CreateSessionAsync должен быть вызван ровно один раз дл€ создани€ сессии нового пользовател€");
    }

    [Test]
    public async Task SignUpAsync_WhenUserExists_ReturnsValidationProblem()
    {
        var request = new SignUpRequestModel
        {
            Login = _faker.Random.String2(6),
            Password = _faker.Random.String2(7),
            Firstname = _faker.Random.String2(5),
            Lastname = _faker.Random.String2(8),
            Description = _faker.Random.String2(100),
            Gender = (int)_faker.PickRandom<GenderEnums>(),
            City = (int)_faker.PickRandom<CityEnum>(),
            Profession = (int)_faker.PickRandom<ProfessionEnums>()
        };

        var passwordService = new PasswordService();
        var hashedPassword = passwordService.EncryptString(request.Password, request.Login);

        var existingUser = new UserEntity
        {
            Id = _faker.Random.Int(1, 200),
            Login = request.Login,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Description = request.Description,
            GenderId = request.Gender,
            CityId = request.City,
            ProfessionId = request.Profession,
            Password = hashedPassword
        };

        _mockMapper.Setup(m => m.Map<UserEntity>(It.IsAny<SignUpRequestModel>()))
                   .Returns(new UserEntity
                   {
                       Login = request.Login,
                       Password = hashedPassword,
                       Firstname = request.Firstname,
                       GenderId = request.Gender,
                       CityId = request.City,
                       ProfessionId = request.Profession
                   });

        _mockUserRepository.Setup(r => r.GetUserByLoginAsync(request.Login, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingUser);

        var result = await _controller.SignUpAsync(request, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;

        Assert.That(objectResult?.Value, Is.InstanceOf<HttpValidationProblemDetails>());

        var problemDetails = objectResult.Value as HttpValidationProblemDetails;
        Assert.That(problemDetails?.Errors, Contains.Key("Login"));
        Assert.That(problemDetails.Errors["Login"], Contains.Item($"ѕользователь с таким логином уже существует"));

        _mockUserRepository.Verify(r => r.CreateUserAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUserRepository.Verify(r => r.CreateSessionAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task SignInAsync_WithInvalidPassword_ReturnsValidationProblem()
    {
        var request = new SignInRequestModel
        {
            Login = _faker.Random.String2(6),
            Password = _faker.Random.String2(10)
        };

        var passwordService = new PasswordService();
        var correctHashedPassword = passwordService.EncryptString(request.Password, request.Login);

        var userEntity = new UserEntity
        {
            Id = _faker.Random.Int(1, 200),
            Login = _faker.Random.String2(10),
            Firstname = _faker.Random.String2(6),
            Password = correctHashedPassword
        };

        _mockUserRepository.Setup(r => r.GetUserByLoginAsync(request.Login, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

        _mockPasswordService.Setup(p => p.VerifyPassword(
                request.Password,
                request.Login,
                userEntity.Password))
            .Returns(false);

        var result = await _controller.SignInAsync(request, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ObjectResult>());

        var objectResult = result as ObjectResult;

        Assert.That(objectResult.Value, Is.InstanceOf<HttpValidationProblemDetails>());

        var problemDetails = objectResult.Value as HttpValidationProblemDetails;
        Assert.That(problemDetails.Errors, Contains.Key("detail"));
        Assert.That(problemDetails.Errors["detail"], Contains.Item("Ќеверно указан логин или пароль пользовател€"));

        _mockUserRepository.Verify(r => r.CreateSessionAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);

        _mockPasswordService.Verify(p => p.VerifyPassword(
            request.Password, request.Login, userEntity.Password), Times.Once);
    }

    [Test]
    public async Task SignInAsync_WithValidCredentials_ReturnsSession()
    {
        var request = new SignInRequestModel
        {
            Login = _faker.Random.String2(6),
            Password = _faker.Random.String2(10)
        };

        var passwordService = new PasswordService();
        var hashedPassword = passwordService.EncryptString(request.Password, request.Login);

        var userEntity = new UserEntity
        {
            Id = _faker.Random.Int(1, 200),
            Login = _faker.Random.String2(10),
            Firstname = _faker.Random.String2(6),
            Password = hashedPassword
        };

        var sessionGuid = Guid.NewGuid();

        _mockUserRepository.Setup(r => r.GetUserByLoginAsync(request.Login, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

        _mockUserRepository.Setup(r => r.CreateSessionAsync(userEntity.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(sessionGuid);

        _mockPasswordService.Setup(p => p.VerifyPassword(
                request.Password,
                request.Login,
                userEntity.Password))
            .Returns(true);

        var result = await _controller.SignInAsync(request, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ContentResult>());
        var contentResult = result as ContentResult;
        Assert.That(contentResult.Content, Is.EqualTo(sessionGuid.ToString()));
        Assert.That(contentResult.ContentType, Is.EqualTo("text/plain"));

        _mockUserRepository.Verify(r => r.GetUserByLoginAsync(request.Login, It.IsAny<CancellationToken>()), Times.Once);
        _mockUserRepository.Verify(r => r.CreateSessionAsync(userEntity.Id, It.IsAny<CancellationToken>()), Times.Once);
        _mockPasswordService.Verify(p => p.VerifyPassword(request.Password, request.Login, userEntity.Password), Times.Once);
    }

    [Test]
    public async Task GetUserByIdAsync_WhenUserExists_ReturnsOkWithUser()
    {
        var userId = _faker.Random.Int(1, 200);
        var passwordService = new PasswordService();
        var hashedPassword = passwordService.EncryptString("password", "testuser");

        var userEntity = new UserEntity
        {
            Id = _faker.Random.Int(1, 200),
            Login = _faker.Random.String2(6),
            Firstname = _faker.Random.String2(5),
            Lastname = _faker.Random.String2(8),
            Description = _faker.Random.String2(100),
            GenderId = (int)_faker.PickRandom<GenderEnums>(),
            CityId = (int)_faker.PickRandom<CityEnum>(),
            ProfessionId = (int)_faker.PickRandom<ProfessionEnums>(),
            Password = hashedPassword
        };

        var userDto = new UserDto
        {
            Id = userId,
            Login = userEntity.Login,
            Firstname = userEntity.Firstname,
            Gender = ((GenderEnums)userEntity.GenderId).ToString(),
            City = ((CityEnum)userEntity.CityId).ToString(),
            Profession = ((ProfessionEnums)userEntity.ProfessionId).ToString(),
            Category = ((ProfessionEnums)userEntity.ProfessionId).ToString(),
            Description = userEntity.Description
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

        _mockMapper.Setup(m => m.Map<UserDto>(userEntity))
                   .Returns(userDto);

        var result = await _controller.GetUserByIdAsync(userId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(userDto));
    }

    [Test]
    public async Task GetUserByIdAsync_WhenUserNotExists_ReturnsNotFound()
    {
        var userId = _faker.Random.Int(1, 200);

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((UserEntity)null);

        var result = await _controller.GetUserByIdAsync(userId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.EqualTo("ѕользователь по указанному идентификатору не найден"));
    }
}