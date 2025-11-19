# PostsController

Контроллер для управления публикациями, комментариями и лайками.

**Базовый URL:** `/Posts`

### Аутентификация
Все методы требуют авторизации

## Методы API

### Публикации
| Метод | Endpoint | Описание | 
|-------|----------|-----------|
| POST | [CreatePostAsync](CreatePostAsync.md) | Создать публикацию |
| GET | [GetPostByIdAsync](GetPostByIdAsync.md) | Получить публикацию по ID | 
| GET | [GetPostsAsync](GetPostsAsync.md) | Получить список публикаций |
| PATCH | [UpdatePostAsync](UpdatePostAsync.md) | Обновить публикацию |
| DELETE | [DeletePostByIdAsync](DeletePostByIdAsync.md) | Удалить публикацию |

### Комментарии
| Метод | Endpoint | Описание | 
|-------|----------|-----------|
| POST | [CreateCommentAsync](CreateCommentAsync.md) | Добавить комментарий к публикации |
| GET | [GetCommentsByPostIdAsync](GetCommentsByPostIdAsync.md) | Получить все комментарии к посту |
| GET | [GetCommentByIdAsync](GetCommentByIdAsync.md) | Получить комментарий по ID |
| POST | [CreateCommentReplyAsync](CreateCommentReplyAsync.md) | Ответить на комментарий |
| PATCH | [UpdateCommentAsync](UpdateCommentAsync.md) | Обновить комментарий |
| DELETE | [DeleteCommentAsync](DeleteCommentAsync.md) | Удалить комментарий | 

### Лайки
| Метод | Endpoint | Описание |
|-------|----------|-----------|
| POST | [CreatePostLikeAsync](CreatePostLikeAsync.md) | Добавить лайк к публикации | 
| POST | [CreateCommentLikeAsync](CreateCommentLikeAsync.md) | Добавить лайк к комментарию | 
| GET | [GetLikeCountByPostIdAsync](GetLikeCountByPostIdAsync.md) | Получить количество лайков к посту |

### Особенности
- Поддержка древовидных комментариев (ответы на комментарии)
- Лайки для постов и комментариев
- Пагинация для списка постов