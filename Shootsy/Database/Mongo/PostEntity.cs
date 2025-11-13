using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Shootsy.Database.Mongo
{
    public class PostEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime EditDate { get; set; } = DateTime.UtcNow;
        public int IdUser { get; set; }
        public required string Text { get; set; }
    }
}
