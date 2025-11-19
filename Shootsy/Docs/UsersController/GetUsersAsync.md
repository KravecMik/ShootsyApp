# Method :: [GET]/Users

## Назначение:
Получение списка пользователей

### Аутентификация:
Требуется авторизация

### Описание параметров запроса:
|Параметр	|Тип	|Обязательный	|Описание| По умолчанию|
|:---------:|:---------:|:----------:  |:----------:  |
|Login|	string|	Нет	|Фильтр по логину|	-|
|Firstname	|string|	Нет|	Фильтр по имени|	-|
|Lastname	|string	|Нет|	Фильтр по фамилии|	-|
|Gender|	string|	Нет|	Фильтр по полу|	-|
|City|	string|	Нет	|Фильтр по городу|	-|
|Profession	|string|	Нет|	Фильтр по профессии|	-|
|Category	|string|	Нет	|Фильтр по категории|	-|
|Page	|int	|Нет	|Номер страницы|	1|
|PageSize	|int|	Нет	|Размер страницы|	10|


### Возможные коды ответов:
|Код|	Описание|	 
|:---------:|:---------:|
|200 OK |	Список пользователей|
| 401 Unauthorized | Пользователь не авторизован |

### Пример ответа:
Пример ответа (200 OK)

```json
[
    {
      "data": [
        {
          "id": 1,
          "createDate": "2024-01-15T10:30:00Z",
          "editDate": "2024-01-15T10:30:00Z",
          "login": "user123",
          "firstname": "Иван",
          "lastname": "Иванов",
          "gender": "Мужской",
          "city": "Москва",
          "profession": "Backend Developer",
          "category": "Development",
          "description": "Разработчик с опытом"
        },
        {
          "id": 74,
          "createDate": "2025-11-15T12:23:18.6360758Z",
          "editDate": "2025-11-16T12:23:18.636135Z",
          "login": "stalkerNoob228",
          "firstname": "Олег",
          "lastname": "Прокин",
          "gender": "Мужчина",
          "city": "Новосибирск",
          "profession": "AQA",
          "category": "Quality Assurance",
          "description": "Люблю шарпы и маму"
        }
      ],
      "page": 1,
      "pageSize": 10,
      "totalCount": 1,
      "totalPages": 1
    }
]
```   

[← Назад к контроллеру](UsersController.md)