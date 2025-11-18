namespace Shootsy.Models.Dtos
{
    public class PostFilterDto
    {
        public int? UserId { get; set; }
        public string? Search { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "CreateDate";
        public bool SortDesc { get; set; } = true;
    }
}