using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shootsy.Database.Mongo
{
    public class FileStorageEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime EditDate { get; set; } = DateTime.UtcNow;
        public int IdUser { get; set; }
        public required FileInfo FileInfo { get; set; }
    }

    public class FileInfo
    {
        public required string FileName { get; set; }
        public required string Extension { get; set; }
        public required string ObjectKey { get; set; }
        public required string ContentPath { get; set; }
    }
}