using Minio;
using Minio.DataModel.Args;
using Microsoft.Extensions.Options;
using Shootsy.Options;

public class MinioStorageService : IObjectStorage
{
    private readonly IMinioClient _client;
    private readonly MinioOptions _opt;

    public MinioStorageService(IOptions<MinioOptions> options)
    {
        _opt = options.Value;
        _client = new MinioClient()
            .WithEndpoint(_opt.Endpoint)
            .WithCredentials(_opt.AccessKey, _opt.SecretKey)
            .WithSSL(_opt.UseSSL)
            .Build();
    }

    public async Task<(string ObjectKey, string? PublicUrl)> UploadAsync(
        Stream stream, string objectKey, string contentType, CancellationToken ct = default)
    {
        var exists = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_opt.Bucket), ct);
        if (!exists)
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_opt.Bucket), ct);

        stream.Position = 0;
        var putArgs = new PutObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(objectKey)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType);

        await _client.PutObjectAsync(putArgs, ct);

        string? publicUrl = null;
        if (!string.IsNullOrEmpty(_opt.PublicBaseUrl))
            publicUrl = $"{_opt.PublicBaseUrl.TrimEnd('/')}/{_opt.Bucket}/{Uri.EscapeDataString(objectKey)}";

        return (objectKey, publicUrl);
    }

    public async Task<string> GetPresignedGetUrlAsync(string objectKey, TimeSpan expires, CancellationToken ct = default)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(objectKey)
            .WithExpiry((int)expires.TotalSeconds);
        return await _client.PresignedGetObjectAsync(args);
    }

    public async Task DeleteAsync(string objectKey, CancellationToken ct = default)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(objectKey);
        await _client.RemoveObjectAsync(args, ct);
    }
}
