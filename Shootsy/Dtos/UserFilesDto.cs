namespace Shootsy.Dtos
{
    public class UserFilesDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FileId { get; set; }

        public bool isActive { get; set; } = false;
    }
}
