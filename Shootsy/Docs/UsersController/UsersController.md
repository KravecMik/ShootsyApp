# UsersController

Контроллер для управления пользователями: регистрация, авторизация и CRUD-операции.

**Базовый URL:** `/Users`

## Методы API:

| Тип запроса | Метод | Описание | Аутентификация |
|-------|----------|-----------|----------------|
| POST |   [SignUpAsync](SignUpAsync.md)| Регистрация пользователя | Не требуется |
| POST | [SignInAsync](SignInAsync.md) | Авторизация пользователя | Не требуется |
| GET | [GetUsersAsync](GetUsersAsync.md) | Получить список пользователей | Требуется |
| GET | [GetUserByIdAsync](GetUserByIdAsync.md) | Получить данные пользователя по ID | Требуется |
| PATCH | [UpdateUserAsync](UpdateUserAsync.md) | Обновить данные пользователя | Требуется |
| DELETE | [DeleteUserByIdAsync](DeleteUserByIdAsync.md) | Удалить пользователя | Требуется |