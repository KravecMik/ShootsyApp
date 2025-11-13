using Shootsy.Models.Enums;

namespace Shootsy.Database.Mongo
{
    public class PostFilterModel
    {
        public int? UserId { get; set; }
        public HashSet<string>? PostIds { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public DateTime? EditDateFrom { get; set; }
        public DateTime? EditDateTo { get; set; }

        public string? Search { get; set; }

        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 100;

        public PostSortByEnum SortBy { get; set; } = PostSortByEnum.CreateDate;
        public bool SortDescending { get; set; } = true;
    }
}
