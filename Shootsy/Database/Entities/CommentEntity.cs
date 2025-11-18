namespace Shootsy.Database.Entities
{
    public class CommentEntity
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public required string Text { get; set; }
        public required int UserId { get; init; }

        public int? ParentCommentId { get; set; }
        public int? PostId { get; set; }

        public UserEntity? User { get; init; }
        public PostEntity? Post { get; init; }
        public CommentEntity? ParentComment { get; init; }

        public ICollection<CommentEntity> Replies { get; init; } = new List<CommentEntity>();
        public ICollection<LikeEntity> Likes { get; init; } = new List<LikeEntity>();
    }
}