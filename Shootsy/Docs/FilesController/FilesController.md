# FilesController

Контроллер для управления файлами в объектном хранилище (MinIO).

**Базовый URL:** `/Files`

### Аутентификация
Все методы требуют авторизации

## Методы API

| Метод | Endpoint | Описание | 
|-------|----------|-----------|
| POST | [CreateFileAsync](CreateFileAsync.md) | Создание карточки файла | 
| GET | [GetFileByIdAsync](GetFileByIdAsync.md) | Получение карточки файла по ID | 
| PATCH | [UpdateFileAsync](UpdateFileAsync.md) | Обновление карточки файла |
| DELETE | [DeleteFileByIdAsync](DeleteFileByIdAsync.md) | Удаление файла по ID |
| GET | [GetFilesListByUserIdAsync](GetFilesListByUserIdAsync.md) | Получить список файлов пользователя |
| DELETE | [DeleteUserFilesAsync](DeleteUserFilesAsync.md) | Удалить все файлы пользователя |

## Технологии
- **Хранилище:** MinIO (S3-совместимое объектное хранилище)
- **База данных:** MongoDB для метаданных файлов
- **Поддерживаемые форматы:** JPG, PNG

[← Назад к контроллеру](FilesController.md)