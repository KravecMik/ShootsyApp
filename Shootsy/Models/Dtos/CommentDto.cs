namespace Shootsy.Models.Dtos
{
    public class CommentDto
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public required string Text { get; set; }
        public required int UserId { get; init; }
        public int? ParentCommentId { get; set; }
        public int? PostId { get; set; }
    }
}