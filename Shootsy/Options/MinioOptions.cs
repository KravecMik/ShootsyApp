namespace Shootsy.Options
{
    public class MinioOptions
    {
        public string Endpoint { get; set; } = default!;
        public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string Bucket { get; set; } = "shootsy";
        public bool UseSSL { get; set; } = false;
        public string? PublicBaseUrl { get; set; }
        public bool MakeBucketPublic { get; set; } = true;
    }
}
