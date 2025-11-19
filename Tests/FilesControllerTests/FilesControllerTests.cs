using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shootsy;
using Shootsy.Controllers;
using Shootsy.Database.Entities;
using Shootsy.Database.Mongo;
using Shootsy.Models.File;
using Shootsy.Repositories;
using System.Text;
namespace Tests.FilesControllerTests;

[TestFixture]
public class FilesControllerTests
{
    private Mock<IFileRepository> _mockFileRepository;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IObjectStorage> _mockObjectStorage;
    private Mock<IMapper> _mockMapper;
    private Mock<InternalConstants> _mockInternalConstants;
    private FilesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockFileRepository = new Mock<IFileRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockObjectStorage = new Mock<IObjectStorage>();
        _mockInternalConstants = new Mock<InternalConstants>();
        _mockMapper = new Mock<IMapper>();

        _mockMapper.Setup(m => m.Map<FileStorageEntity>(It.IsAny<FileStorageEntity>()))
                  .Returns<FileStorageEntity>(entity => new FileStorageEntity
                  {
                      Id = entity.Id,
                      UserId = entity.UserId,
                      FileInfo = entity.FileInfo
                  });

        _mockMapper.Setup(m => m.Map<IEnumerable<FileStorageEntity>>(It.IsAny<IEnumerable<FileStorageEntity>>()))
                  .Returns<IEnumerable<FileStorageEntity>>(entities => entities.Select(entity => new FileStorageEntity
                  {
                      Id = entity.Id,
                      UserId = entity.UserId,
                      FileInfo = entity.FileInfo
                  }));

        _controller = new FilesController(
            _mockFileRepository.Object,
            _mockUserRepository.Object,
            _mockInternalConstants.Object,
            _mockMapper.Object,
            _mockObjectStorage.Object
        );

        var sessionGuid = Guid.NewGuid();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Session"] = sessionGuid.ToString();

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var userEntity = new UserEntity
        {
            Id = 123,
            Login = "testuser",
            Firstname = "Test",
            Password = new byte[10]
        };

        _mockUserRepository.Setup(r => r.GetUserByGuidAsync(sessionGuid, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);
    }

    [Test]
    public async Task CreateFileAsync_WithValidFile_ReturnsCreatedResult()
    {
        var fileContent = "test file content";
        var fileName = "test.jpg";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        var formFile = new FormFile(stream, 0, stream.Length, "File", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpg"
        };

        var request = new CreateFileRequestModel { File = formFile };
        var userId = 123;
        var fileId = "507f1f77bcf86cd799439011";
        var objectKey = $"{userId}/guid/{fileName}";
        var publicUrl = $"https://storage.example.com/bucket/{objectKey}";

        var userEntity = new UserEntity { Id = userId, Login = "testuser", Firstname = "gdf", Password = new byte[5] };

        _mockUserRepository.Setup(r => r.GetUserByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

        _mockObjectStorage.Setup(o => o.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((objectKey, publicUrl));

        _mockFileRepository.Setup(r => r.CreateFileAsync(It.IsAny<FileStorageEntity>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(fileId);

        var result = await _controller.CreateFileAsync(request, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(201));
        Assert.That(objectResult.Value, Is.EqualTo(fileId));

        _mockUserRepository.Verify(r => r.GetUserByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetUserByGuidAsync должен быть вызван ровно один раз для получения текущего пользователя");

        _mockObjectStorage.Verify(o => o.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once,
            "Метод UploadAsync должен быть вызван ровно один раз для загрузки файла в объектное хранилище");

        _mockFileRepository.Verify(r => r.CreateFileAsync(It.IsAny<FileStorageEntity>(), It.IsAny<CancellationToken>()), Times.Once,
            "Метод CreateFileAsync должен быть вызван ровно один раз для сохранения метаданных файла в базу данных");
    }

    [Test]
    public async Task CreateFileAsync_WithUnsupportedExtension_ReturnsValidationProblem()
    {
        var fileContent = "test file content";
        var fileName = "test.exe";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        var formFile = new FormFile(stream, 0, stream.Length, "File", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/exe"
        };

        var request = new CreateFileRequestModel { File = formFile };
        var userEntity = new UserEntity { Id = 123, Login = "testuser", Firstname = "gdf", Password = new byte[5] };

        _mockUserRepository.Setup(r => r.GetUserByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

        var result = await _controller.CreateFileAsync(request, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;

        Assert.That(objectResult.Value, Is.InstanceOf<HttpValidationProblemDetails>());
        var problemDetails = objectResult.Value as HttpValidationProblemDetails;
        Assert.That(problemDetails.Errors, Contains.Key("File.Extension"));

        _mockObjectStorage.Verify(o => o.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never,
            "Метод UploadAsync не должен вызываться для файлов с неподдерживаемым расширением");

        _mockFileRepository.Verify(r => r.CreateFileAsync(It.IsAny<FileStorageEntity>(), It.IsAny<CancellationToken>()), Times.Never,
            "Метод CreateFileAsync не должен вызываться для файлов с неподдерживаемым расширением");
    }

    [Test]
    public async Task GetFileByIdAsync_WhenFileExists_ReturnsOkWithFile()
    {
        var fileId = "507f1f77bcf86cd799439011";
        var fileEntity = new FileStorageEntity
        {
            Id = fileId,
            UserId = 123,
            FileInfo = new Shootsy.Database.Mongo.FileInfo
            {
                FileName = "test.jpg",
                Extension = ".jpg",
                ObjectKey = "123/guid/test.jpg",
                ContentPath = "https://storage.example.com/bucket/123/guid/test.jpg"
            }
        };

        _mockFileRepository.Setup(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(fileEntity);

        var result = await _controller.GetFileByIdAsync(fileId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;

        Assert.That(okResult.Value, Is.EqualTo(fileEntity));

        _mockFileRepository.Verify(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetFileByIdAsync должен быть вызван ровно один раз для получения файла по идентификатору");
    }

    [Test]
    public async Task GetFileByIdAsync_WhenFileNotExists_ReturnsNotFound()
    {
        var fileId = "nonexistent";

        _mockFileRepository.Setup(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((FileStorageEntity)null);

        var result = await _controller.GetFileByIdAsync(fileId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());

        _mockFileRepository.Verify(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetFileByIdAsync должен быть вызван ровно один раз для попытки найти файл");
    }

    [Test]
    public async Task UpdateFileAsync_WithValidPatch_ReturnsNoContent()
    {
        var fileId = "507f1f77bcf86cd799439011";
        var fileEntity = new FileStorageEntity
        {
            Id = fileId,
            UserId = 123,
            FileInfo = new Shootsy.Database.Mongo.FileInfo
            {
                FileName = "oldname.jpg",
                Extension = ".jpg",
                ObjectKey = "123/guid/oldname.jpg",
                ContentPath = "https://storage.example.com/bucket/123/guid/oldname.jpg"
            }
        };

        var patch = new JsonPatchDocument<FileStorageEntity>();
        patch.Replace(f => f.FileInfo.FileName, "newname.jpg");

        _mockFileRepository.Setup(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(fileEntity);

        _mockFileRepository.Setup(r => r.UpdateFileAsync(fileEntity, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

        var result = await _controller.UpdateFileAsync(fileId, patch, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<NoContentResult>());

        _mockFileRepository.Verify(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetFileByIdAsync должен быть вызван ровно один раз для получения файла перед обновлением");

        _mockFileRepository.Verify(r => r.UpdateFileAsync(fileEntity, It.IsAny<CancellationToken>()), Times.Once,
            "Метод UpdateFileAsync должен быть вызван ровно один раз для сохранения обновленных данных файла");
    }

    [Test]
    public async Task UpdateFileAsync_WhenTryingToChangeObjectKey_ReturnsValidationProblem()
    {
        var fileId = "507f1f77bcf86cd799439011";
        var fileEntity = new FileStorageEntity
        {
            Id = fileId,
            UserId = 123,
            FileInfo = new Shootsy.Database.Mongo.FileInfo
            {
                FileName = "test.jpg",
                Extension = ".jpg",
                ObjectKey = "123/guid/test.jpg",
                ContentPath = "https://storage.example.com/bucket/123/guid/test.jpg"
            }
        };

        var patch = new JsonPatchDocument<FileStorageEntity>();
        patch.Replace(f => f.FileInfo.ObjectKey, "new/key.jpg");

        _mockFileRepository.Setup(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(fileEntity);

        var result = await _controller.UpdateFileAsync(fileId, patch, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;

        Assert.That(objectResult.Value, Is.InstanceOf<HttpValidationProblemDetails>());
        var problemDetails = objectResult.Value as HttpValidationProblemDetails;
        Assert.That(problemDetails.Errors, Contains.Key("ObjectKey"));

        _mockFileRepository.Verify(r => r.UpdateFileAsync(It.IsAny<FileStorageEntity>(), It.IsAny<CancellationToken>()), Times.Never,
            "Метод UpdateFileAsync не должен вызываться при попытке изменить запрещенные поля");
    }

    [Test]
    public async Task DeleteFileByIdAsync_WhenFileExists_ReturnsNoContent()
    {
        var fileId = "507f1f77bcf86cd799439011";
        var fileEntity = new FileStorageEntity
        {
            Id = fileId,
            UserId = 123,
            FileInfo = new Shootsy.Database.Mongo.FileInfo
            {
                FileName = "test.jpg",
                Extension = ".jpg",
                ObjectKey = "123/guid/test.jpg",
                ContentPath = "https://storage.example.com/bucket/123/guid/test.jpg"
            }
        };

        _mockFileRepository.Setup(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(fileEntity);

        _mockObjectStorage.Setup(o => o.DeleteAsync(fileEntity.FileInfo.ObjectKey, It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

        _mockFileRepository.Setup(r => r.DeleteFileByIdAsync(fileId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

        var result = await _controller.DeleteFileByIdAsync(fileId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<NoContentResult>());

        _mockFileRepository.Verify(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetFileByIdAsync должен быть вызван ровно один раз для получения файла перед удалением");

        _mockObjectStorage.Verify(o => o.DeleteAsync(fileEntity.FileInfo.ObjectKey, It.IsAny<CancellationToken>()), Times.Once,
            "Метод DeleteAsync должен быть вызван ровно один раз для удаления файла из объектного хранилища");

        _mockFileRepository.Verify(r => r.DeleteFileByIdAsync(fileId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод DeleteFileByIdAsync должен быть вызван ровно один раз для удаления метаданных файла из базы данных");
    }

    [Test]
    public async Task DeleteFileByIdAsync_WhenFileNotExists_ReturnsNotFound()
    {
        var fileId = "nonexistent";

        _mockFileRepository.Setup(r => r.GetFileByIdAsync(fileId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((FileStorageEntity)null);

        var result = await _controller.DeleteFileByIdAsync(fileId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());

        _mockObjectStorage.Verify(o => o.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never,
            "Метод DeleteAsync не должен вызываться если файл не найден");

        _mockFileRepository.Verify(r => r.DeleteFileByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never,
            "Метод DeleteFileByIdAsync не должен вызываться если файл не найден");
    }

    [Test]
    public async Task GetFilesListByUserIdAsync_ReturnsFilesList()
    {
        var userId = 123;
        var filesList = new List<FileStorageEntity>
    {
        new FileStorageEntity
        {
            Id = "file1",
            UserId = userId,
            FileInfo = new Shootsy.Database.Mongo.FileInfo
            {
                FileName = "file1.jpg",
                Extension = "jpg",
                ObjectKey = "hfg",
                ContentPath = "hfg"
            }
        },
        new FileStorageEntity
        {
            Id = "file2",
            UserId = userId,
            FileInfo = new Shootsy.Database.Mongo.FileInfo
            {
                FileName = "file1.jpg",
                Extension = "jpg",
                ObjectKey = "hfg",
                ContentPath = "hfg"
            }
        }
    };

        var responseList = new List<FileStorageEntity>
        { 
            new FileStorageEntity {
                Id = "file1",
                UserId = userId,
                FileInfo = new Shootsy.Database.Mongo.FileInfo
                {
                    FileName = "file1.jpg",
                    Extension = "jpg",
                    ObjectKey = "hfg",
                    ContentPath = "hfg"
                }
                },
            new FileStorageEntity {
                Id = "file1",
                UserId = userId,
                FileInfo = new Shootsy.Database.Mongo.FileInfo
                {
                    FileName = "file1.jpg",
                    Extension = "jpg",
                    ObjectKey = "hfg",
                    ContentPath = "hfg"
                }
            }
        };

        _mockFileRepository.Setup(r => r.GetFilesListByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(filesList);

        var result = await _controller.GetFilesListByUserIdAsync(userId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(filesList));

        _mockFileRepository.Verify(r => r.GetFilesListByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetFilesListByUserIdAsync должен быть вызван ровно один раз для получения списка файлов пользователя");
    }

    [Test]
    public async Task DeleteUserFilesAsync_WhenUserExists_DeletesAllUserFiles()
    {
        var userId = 123;
        var userEntity = new UserEntity { Id = userId, Login = "testuser", Firstname = "gdf", Password = new byte[5] };

        var userFiles = new List<FileStorageEntity>
        {
            new FileStorageEntity
            {
                Id = "file1",
                UserId = userId,
                FileInfo = new Shootsy.Database.Mongo.FileInfo
                {
                    ObjectKey = "file1.jpg",
                    Extension = "jpg",
                    ContentPath = "hfg",
                    FileName = "lol"
                }
            },
            new FileStorageEntity
            {
                Id = "file2",
                UserId = userId,
                FileInfo = new Shootsy.Database.Mongo.FileInfo
                {
                    ObjectKey = "file2.jpg",
                    Extension = "jpg",
                    ContentPath = "hfg",
                    FileName = "lol"
                }
            }
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

        _mockFileRepository.Setup(r => r.GetFilesListByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userFiles);

        _mockObjectStorage.Setup(o => o.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

        _mockFileRepository.Setup(r => r.DeleteFileByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

        var result = await _controller.DeleteUserFilesAsync(userId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<NoContentResult>());

        _mockUserRepository.Verify(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetUserByIdAsync должен быть вызван ровно один раз для проверки существования пользователя");

        _mockFileRepository.Verify(r => r.GetFilesListByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once,
            "Метод GetFilesListByUserIdAsync должен быть вызван ровно один раз для получения списка файлов пользователя");

        _mockObjectStorage.Verify(o => o.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(2),
            "Метод DeleteAsync должен быть вызван для каждого файла пользователя в объектном хранилище");

        _mockFileRepository.Verify(r => r.DeleteFileByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(2),
            "Метод DeleteFileByIdAsync должен быть вызван для каждого файла пользователя в базе данных");
    }

    [Test]
    public async Task DeleteUserFilesAsync_WhenUserNotExists_ReturnsNotFound()
    {
        var userId = 999;

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((UserEntity)null);

        var result = await _controller.DeleteUserFilesAsync(userId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());

        _mockFileRepository.Verify(r => r.GetFilesListByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never,
            "Метод GetFilesListByUserIdAsync не должен вызываться если пользователь не найден");

        _mockObjectStorage.Verify(o => o.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never,
            "Метод DeleteAsync не должен вызываться если пользователь не найден");
    }
}