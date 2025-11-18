namespace Shootsy.Database.Entities
{
    public class PostEntity
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public required int UserId { get; init; }
        public required string Text { get; init; }

        public UserEntity? User { get; init; }
        public ICollection<CommentEntity> Comments { get; init; } = new List<CommentEntity>();
        public ICollection<LikeEntity> Likes { get; init; } = new List<LikeEntity>();
    }
}