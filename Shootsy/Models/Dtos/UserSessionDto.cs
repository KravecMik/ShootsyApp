namespace Shootsy.Models.Dtos
{
    public class UserSessionDto
    {
        public int Id { get; init; }
        public int User { get; init; }
        public DateTime SessionDateFrom { get; set; }
        public DateTime SessionDateTo { get; set; }
        public Guid Guid { get; init; }
    }
}