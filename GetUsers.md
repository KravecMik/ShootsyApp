# Method :: [GET]/Users/GetUsers

## Назначение

Получение списка информации о пользователях

## Входные данные

```json
{
    "offset": int, //Сколько записей пропустить -- По умолчанию 0 
    "limit": int, //Сколько записей вернуть -- Required
    "filter": string, //Условия фильтрации записей -- По умолчанию "id > 0"
    "sort": string //Способ сортировки записей -- По умолчанию "id desc"
}
```

## Выходные данные
```json
[
    {
        "id": 3,
        "createDate": "2024-09-04T07:37:34.591718Z",
        "editDate": "2024-09-04T07:37:34.591718Z",
        "login": "2",
        "gender": "Женский",
        "city": "Барнаул",
        "firstname": "Витек",
        "lastname": "Кириешкин",
        "cooperationType": "Не указано",
        "type": "Модель",
        "contact": "аська",
        "patronymic": null,
        "fullname": "Витек Кириешкин",
        "discription": "Рецепт супа крайне прост. Просто свари суп лалка",
        "avatar": "",
        "isNude": false
    },
    {
        "id": 2,
        "createDate": "2024-09-04T07:37:07.488074Z",
        "editDate": "2024-09-04T07:37:07.488074Z",
        "login": "1",
        "gender": "Мужской",
        "city": "Новосибирск",
        "firstname": "Алабай",
        "lastname": "Кириешкин",
        "cooperationType": "Не указано",
        "type": "Фотограф",
        "contact": "@kravectv",
        "patronymic": null,
        "fullname": "Алабай Кириешкин",
        "discription": "Рецепт супа крайне прост. Просто свари суп лалка",
        "avatar": "",
        "isNude": false
    }
]
```

## Пример вызова

```bash
curl -L -X GET 'http://38.135.55.111:5000/Users/?sort=id asc&filter=gender eq 2&limit=5'
```

## Пример ответа

200

```bash
[
    {
        "id": 3,
        "createDate": "2024-09-04T07:37:34.591718Z",
        "editDate": "2024-09-04T07:37:34.591718Z",
        "login": "2",
        "gender": "Женский",
        "city": "Барнаул",
        "firstname": "Витек",
        "lastname": "Кириешкин",
        "cooperationType": "Не указано",
        "type": "Модель",
        "contact": "аська",
        "patronymic": null,
        "fullname": "Витек Кириешкин",
        "discription": "Рецепт супа крайне прост. Просто свари суп лалка",
        "avatar": "",
        "isNude": false
    }
]
```

## Алгоритм
1. Проверить заполнение обязательных полей. Если поля не заполнены - вернуть BadRequest
2. Проверить наличие и валидность хэдера session в запросе. В случае неудачи вернуть Unauthorized.
3. Вернуть список пользователей. Если пользователи не найдены вернуть пустой массив.