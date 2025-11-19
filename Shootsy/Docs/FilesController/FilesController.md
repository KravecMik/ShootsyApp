# FilesController

Контроллер для управления файлами в объектном хранилище (MinIO).

**Базовый URL:** `/Files`

### Аутентификация
Все методы требуют авторизации

## Методы API

| Метод | Endpoint | Описание | Аутентификация |
|-------|----------|-----------|----------------|
| POST | [CreateFileAsync](CreateFileAsync.md) | Создание карточки файла | Требуется |
| GET | [GetFileByIdAsync](GetFileByIdAsync.md) | Получение карточки файла по ID | Требуется |
| PATCH | [UpdateFileAsync](UpdateFileAsync.md) | Обновление карточки файла | Требуется |
| DELETE | [DeleteFileByIdAsync](DeleteFileByIdAsync.md) | Удаление файла по ID | Требуется |
| GET | [GetFilesListByUserIdAsync](GetFilesListByUserIdAsync.md) | Получить список файлов пользователя | Требуется |
| DELETE | [DeleteUserFilesAsync](DeleteUserFilesAsync.md) | Удалить все файлы пользователя | Требуется |

## Технологии
- **Хранилище:** MinIO (S3-совместимое объектное хранилище)
- **База данных:** MongoDB для метаданных файлов
- **Поддерживаемые форматы:** JPG, PNG

[← Назад к контроллеру](FilesController.md)