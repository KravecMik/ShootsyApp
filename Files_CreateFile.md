# Method :: [POST]/Files

## Назначение:
Загрузка файла пользователя.

## Входные данные:

### Тело запроса (multipart/form-data):
```
user: string         // Логин пользователя-владельца файла (обязателен)
file: IFormFile      // Файл (обязателен)
extension: string    // Расширение файла, например "jpg" (обязателен)
```

## Успешный ответ: 200 OK
```json
{
  "id": 123,
  "createDate": "2024-09-04T07:37:07.488074Z",
  "editDate": "2024-09-04T07:37:07.488074Z",
  "user": "demo",
  "extension": "jpg",
  "contentPath": "/content/files/123.jpg"
}
```
