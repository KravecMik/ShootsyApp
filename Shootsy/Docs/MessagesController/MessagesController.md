# MessagesController

Контроллер для отправки сообщений между пользователями через Kafka.

**Базовый URL:** `/Messages`

## Методы API

| Метод | Endpoint | Описание | Аутентификация |
|-------|----------|-----------|----------------|
| POST | [SendMessageByUserLoginAsync](SendMessageByUserLoginAsync.md) | Отправить сообщение пользователю по логину | Требуется |