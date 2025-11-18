namespace Shootsy.Database.Entities
{
    public class LikeEntity
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public required int UserId { get; set; }


        public PostEntity? Post { get; init; }
        public CommentEntity? Comment { get; init; }
        public UserEntity? User { get; init; }
    }
}