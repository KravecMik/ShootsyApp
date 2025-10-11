public interface IObjectStorage
{
    /// <summary>Загрузить объект и вернуть его ключ (и, опционально, публичный URL).</summary>
    Task<(string ObjectKey, string? PublicUrl)> UploadAsync(
        Stream stream, string objectKey, string contentType, CancellationToken ct = default);

    /// <summary>Получить presigned-URL для приватного объекта.</summary>
    Task<string> GetPresignedGetUrlAsync(string objectKey, TimeSpan expires, CancellationToken ct = default);

    /// <summary>Удалить объект.</summary>
    Task DeleteAsync(string objectKey, CancellationToken ct = default);
}