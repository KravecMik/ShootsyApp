# Method :: [GET]/Files/[fileId]

**Получение карточки файла по идентификатору**

## Аутентификация
Требуется авторизация

## Параметры запроса

| Параметр | Тип | Обязательный | Описание |
|----------|-----|--------------|-----------|
| fileId | string | Да | ID файла в MongoDB |

## Ответы

| Код | Описание | 
|-----|-----------|
| 200 OK | Данные файла найдены |
| 404 NotFound | Файл не найден | 
| 401 Unauthorized | Пользователь не авторизован |

## Пример ответа (200 OK)
```json
{
  "id": "507f1f77bcf86cd799439011",
  "createDate": "2024-01-15T10:30:00Z",
  "editDate": "2024-01-15T10:30:00Z",
  "userId": 123,
  "fileInfo": {
    "fileName": "image.jpg",
    "extension": ".jpg",
    "objectKey": "123/a1b2c3d4e5f6/image.jpg",
    "contentPath": "https://minio.example.com/bucket/123/a1b2c3d4e5f6/image.jpg"
  }
}
```

[← Назад к контроллеру](FilesController.md)