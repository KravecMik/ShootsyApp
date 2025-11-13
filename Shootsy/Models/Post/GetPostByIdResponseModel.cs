namespace Shootsy.Models.Post
{
    public class GetPostByIdResponseModel
    {
        public required string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int IdUser { get; set; }
        public required string Text { get; set; }
    }
}
