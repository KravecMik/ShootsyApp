# Методы взаимодействия

# Users
*[POST] [CreateUser -- *Создание пользователя*](CreateUser.md)<br>
*[POST] Authorization <br>
*[GET] [GetUserById -- *Получение информации о пользователе по идентификатору*](GetUserById.md)<br>
*[GET] GetUsers <br>
*[PATCH] UpdateUser <br>
*[DELETE] DeleteUserById

# UserSessions
*[GET] GetLastUserSession <br>
*[GET] CheckUserAccess

# Files
*[POST] CreateFile <br>
*[GET] GetFileById <br>
*[GET] GetFiles <br>
*[PATCH] UpdateFile <br>
*[DELETE] DeleteFileById <br>
*[DELETE] DeleteManyFiles

## Перечисления

### Города
|Идентификатор|Навание города|
|:---------:|:---------:|
|1|Новосибирск|
|2|Барнаул|

### Тип учетной записи
|Идентификатор|Тип учетной записи|
|:---------:|:---------:|
|1|Фотограф|
|2|Модель|

### Тип сотрудничества
|Идентификатор|Тип сотрудничества|
|:---------:|:---------:|
|1|Не указано|
|2|Расходы оплачивает модель|
|3|Расходы оплачивает фотограф|
|4|Расходы оплачиваются поровну|

### Пол
|Идентификатор|Тип учетной записи|
|:---------:|:---------:|
|1|Мужской|
|2|Женский|

# Files
*[POST] [CreateFile](Files_CreateFile.md) <br>
*[GET] [GetFileById](Files_GetFileById.md) <br>
*[GET] [GetFiles](Files_GetFiles.md) <br>
*[DELETE] [DeleteFileById](Files_DeleteFileById.md) <br>
*[DELETE] [DeleteManyFiles](Files_DeleteManyFiles.md) <br>

# Service
*[GET] [Health](Service_Health.md) <br>
*[POST] [EnsureCreated](Service_EnsureCreated.md) <br>
*[DELETE] [EnsureDeleted](Service_EnsureDeleted.md) <br>
