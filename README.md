# Методы взаимодействия

# Users
*[POST] [CreateUserAsync -- *Создание пользователя*](CreateUser.md)<br>
*[GET] [GetUserByIdAsync -- *Получение информации о пользователе по идентификатору*](GetUserById.md)<br>
*[GET] GetUsersAsync <br>
*[PATCH] UpdateUserAsync <br>
*[DELETE] DeleteUserByIdAsync

# Files
*[POST] CreateFileAsync <br>
*[GET] GetFileByIdAsync <br>
*[GET] GetFilesAsync <br>
*[PATCH] UpdateFileAsync <br>
*[DELETE] DeleteFileByIdAsync
*[DELETE] DeleteManyFilesAsync

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
