namespace Shootsy.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public required string Login { get; set; }
        public required string Firstname { get; set; }
        public string? Lastname { get; set; }
        public required string Gender { get; set; }
        public required string City { get; set; }
        public required string Profession { get; set; }
        public required string Category { get; set; }
        public string? Description { get; set; }
    }
}