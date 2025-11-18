namespace Shootsy.Models.Dtos
{
    public class UserFilterDto
    {
        public string? City { get; set; }
        public string? Profession { get; set; }
        public string? Category { get; set; }
        public string? Gender { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}