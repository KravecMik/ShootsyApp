namespace Shootsy.Models.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}