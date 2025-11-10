using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Shootsy.Options;

public class MinioInitializer : IHostedService
{
    private readonly IMinioClient _client;
    private readonly MinioOptions _opt;
    private readonly ILogger<MinioInitializer> _log;

    public MinioInitializer(IOptions<MinioOptions> options, ILogger<MinioInitializer> log)
    {
        _opt = options.Value;
        _log = log;
        _client = new MinioClient()
            .WithEndpoint(_opt.Endpoint)
            .WithCredentials(_opt.AccessKey, _opt.SecretKey)
            .WithSSL(_opt.UseSSL)
            .WithTimeout(60000)
            .Build();
    }

    public async Task StartAsync(CancellationToken ct)
    {
        // 1) Бакет
        var exists = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_opt.Bucket), ct);
        if (!exists)
        {
            _log.LogInformation("MinIO: creating bucket {Bucket}", _opt.Bucket);
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_opt.Bucket), ct);
        }

        // 2) Публичное чтение (анонимам) — корректный ARN!
        if (_opt.MakeBucketPublic)
        {
            var policyJson = $@"{{
              ""Version"": ""2012-10-17"",
              ""Statement"": [
                {{
                  ""Effect"": ""Allow"",
                  ""Principal"": ""*"",
                  ""Action"": [ ""s3:GetObject"" ],
                  ""Resource"": [ ""arn:aws:s3:::{_opt.Bucket}/*"" ]
                }}
              ]
            }}";

            _log.LogInformation("MinIO: setting public-read policy on {Bucket}", _opt.Bucket);

            await _client.SetPolicyAsync(
                new SetPolicyArgs()
                    .WithBucket(_opt.Bucket)
                    .WithPolicy(policyJson),
                ct);
        }

        _log.LogInformation("MinIO: bucket {Bucket} is ready", _opt.Bucket);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}