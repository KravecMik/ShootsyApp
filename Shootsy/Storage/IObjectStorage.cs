public interface IObjectStorage
{
    Task<(string ObjectKey, string? PublicUrl)> UploadAsync(Stream stream, string objectKey, string contentType, CancellationToken ct = default);
    Task<string> GetPresignedGetUrlAsync(string objectKey, TimeSpan expires, CancellationToken ct = default);
    Task DeleteAsync(string objectKey, CancellationToken ct = default);
}