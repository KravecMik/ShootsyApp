namespace Shootsy.Options;

public class MongoOptions
{
    public string ConnectionString { get; set; } = default!;
    public string Database { get; set; } = default!;
    public string CardsCollection { get; set; } = "FileStorage";
}