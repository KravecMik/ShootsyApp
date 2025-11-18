namespace Shootsy.Models.Dtos
{
    public class LikeDto
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public required int UserId { get; set; }
    }
}