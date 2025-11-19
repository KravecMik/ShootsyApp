# PostsController

Контроллер для управления публикациями, комментариями и лайками.

**Базовый URL:** `/Posts`

### Аутентификация
Все методы требуют авторизации (`[Authorize]`)

## Методы API

### Публикации
| Метод | Endpoint | Описание | Аутентификация |
|-------|----------|-----------|----------------|
| POST | [/](create-post.md) | Создать публикацию | Требуется |
| GET | [/{postId}](get-post-by-id.md) | Получить публикацию по ID | Требуется |
| GET | [/?](get-posts.md) | Получить список публикаций | Требуется |
| PATCH | [/{postId}](update-post.md) | Обновить публикацию | Требуется |
| DELETE | [/{postId}](delete-post.md) | Удалить публикацию | Требуется |

### Комментарии
| Метод | Endpoint | Описание | Аутентификация |
|-------|----------|-----------|----------------|
| POST | [/{postId}/comments](add-comment.md) | Добавить комментарий к публикации | Требуется |
| GET | [/{postId}/comments](get-comments.md) | Получить все комментарии к посту | Требуется |
| GET | [/{postId}/comments/{commentId}](get-comment.md) | Получить комментарий по ID | Требуется |
| POST | [/{postId}/comments/{commentId}](add-comment-reply.md) | Ответить на комментарий | Требуется |
| PATCH | [/{postId}/comments/{commentId}](update-comment.md) | Обновить комментарий | Требуется |
| DELETE | [/{postId}/comments/{commentId}](delete-comment.md) | Удалить комментарий | Требуется |

### Лайки
| Метод | Endpoint | Описание | Аутентификация |
|-------|----------|-----------|----------------|
| POST | [/{postId}/likes](add-post-like.md) | Добавить лайк к публикации | Требуется |
| POST | [/{postId}/comments/{commentId}/likes](add-comment-like.md) | Добавить лайк к комментарию | Требуется |
| GET | [/{postId}/likes/count](get-likes-count.md) | Получить количество лайков к посту | Требуется |

## Модели данных
- [CreatePostRequestModel](../models/CreatePostRequestModel.md)
- [PostDto](../models/PostDto.md)
- [CommentDto](../models/CommentDto.md)
- [AddCommentRequestModel](../models/AddCommentRequestModel.md)

### Особенности
- Поддержка древовидных комментариев (ответы на комментарии)
- Лайки для постов и комментариев
- Пагинация для списка постов