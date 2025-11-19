# GET /Files/linked-user/[userId]

**Получить список файлов пользователя**

## Аутентификация
Требуется авторизация

## Параметры запроса

| Параметр | Тип | Обязательный | Описание |
|----------|-----|--------------|-----------|
| userId | int | Да | ID пользователя |

## Ответы

| Код | Описание | 
|-----|-----------|
| 200 OK | Список файлов пользователя |
| 401 Unauthorized | Пользователь не авторизован |

## Пример ответа (200 OK)
```json
[
  {
    "id": "507f1f77bcf86cd799439011",
    "createDate": "2024-01-15T10:30:00Z",
    "editDate": "2024-01-15T10:30:00Z", 
    "userId": 123,
    "fileInfo": {
      "fileName": "image1.jpg",
      "extension": ".jpg",
      "objectKey": "123/a1b2c3d4e5f6/image1.jpg",
      "contentPath": "https://minio.example.com/bucket/123/a1b2c3d4e5f6/image1.jpg"
    }
  },
  {
    "id": "507f1f77bcf86cd799439012",
    "createDate": "2024-01-16T11:30:00Z",
    "editDate": "2024-01-16T11:30:00Z",
    "userId": 123,
    "fileInfo": {
      "fileName": "image2.png", 
      "extension": ".png",
      "objectKey": "123/b2c3d4e5f6g7/image2.png",
      "contentPath": "https://minio.example.com/bucket/123/b2c3d4e5f6g7/image2.png"
    }
  }
]
```

[← Назад к контроллеру](FilesController.md)