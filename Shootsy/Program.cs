using Shootsy.Database;
using Shootsy.Database.Entities;

using (ApplicationContext db = new ApplicationContext())
{
    UserTypeEntity userTypePhotograph = new UserTypeEntity{ Type = "Фотограф" };
    UserTypeEntity userTypeModel = new UserTypeEntity{ Type = "Модель" };


    UserEntity user1 = new UserEntity { Login = "Test", Type = userTypePhotograph.Id, Contact = "Telega" };
    UserEntity user2 = new UserEntity { Login = "Test 2", Type = userTypeModel.Id, Contact = "Wazap" };


    UserSessionEntity userSession = new UserSessionEntity
    {
        UserId = user1.Id,
        Guid = Guid.NewGuid(),
        CreateDate = DateTime.UtcNow,
    };

    // добавляем их в бд
    db.UserTypes.AddRange(userTypePhotograph, userTypeModel);
    db.Users.AddRange(user1, user2);
    //db.UserSession.Add(userSession);
    db.SaveChanges();
    Console.WriteLine("Объекты успешно сохранены");

    // получаем объекты из бд и выводим на консоль
    var users = db.Users.ToList();
    Console.WriteLine("Список объектов:");
    foreach (UserEntity u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Login} - {u.CreateDate}");
    }
}
