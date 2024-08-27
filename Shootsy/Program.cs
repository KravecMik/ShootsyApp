using Shootsy.Database;
using Shootsy.Database.Entities;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

using (ApplicationContext db = new ApplicationContext())
{

    UserEntity user1 = new UserEntity { Login = "Test", TypeId = 2, Firstname = "Олега", Lastname = "Прокин", Password = new byte[5], CityId = 1, GenderId = 1 };
    UserEntity user2 = new UserEntity { Login = "Test 2", TypeId = 1, Firstname = "Миха", Lastname = "Кравец", Password = new byte[5], CityId = 2, GenderId = 2 };


    UserSessionEntity userSession = new UserSessionEntity
    {
        User = user1,
        Guid = Guid.NewGuid(),
        CreateDate = DateTime.UtcNow,
    };

    // добавляем их в бд
    db.Users.AddRange(user1, user2);
    db.UserSession.Add(userSession);
    db.SaveChanges();
}
