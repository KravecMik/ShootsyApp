# Method :: [GET]/Users/GetUserById

## Назначение

Получение информации о пользователе по идентификатору   

## Входные данные

```json
{
    "id": int //Идентификатор пользователя
}
```

## Выходные данные
```json
{
    "Id": int, //Идентификатор
    "Login": "string", //Логин
    "Firstname": "string", //Имя
    "Lastname": "string", //Фамилия
    "Patronymic": "string", //Отчество
    "Fullname": "string", //Фамилия Имя Отчество (при наличии)
    "Gender": "string", //Пол
    "Password": byte[], //Хэш пароля пользователя
    "City": "string", //Город проживания
    "Discription": "string", //Описание профиля
    "Contact": "string", //Контакт для связи
    "CooperationType": "string", //Тип сотрудничества
    "Type": "string", //Тип учетной записи
    "isNude": bool, //Участвует ли пользователь в ню съемках,
    "isHasActiveSubscribe": bool, //Активна ли у пользователя подписка,
    "CreateDate": "DateTime", //Дата создания пользователя
    "EditDate": "DateTime" //Дата редактирования пользователя
}
```

## Пример вызова

```bash
curl -L -X GET 'http://38.135.55.111:5000/Users/1'
```

## Пример ответа

200

```bash
{
  "Id": 1,
  "Login": "156",
  "Gender": Мужской,
  "City": Новосибирск,
  "Contact": "@kravectv",
  "Firstname": "Алабай",
  "Lastname": "Кириешкин",
  "Fullname": "Алабай Кириешкин",
  "Discription": "Описание моего лучшего профиля",
  "CooperationTypeId": "Расходы оплачивает модель",
  "Password": {4 55 6 23},
  "TypeId": "Фотограф",
  "isNude": false,
  "isHasActiveSubscribe": false,
  "CreateDate": "2024-08-29T04:59:46.267916Z",
  "EditDate": "2024-08-29T04:59:46.287895Z"
}
```

## Алгоритм

1. Проверить наличие информации о пользователя в БД.<br> 
   1.1 В случае если пользователь не найден, вернуть NotFound
2. Вернуть информацию о пользователе

