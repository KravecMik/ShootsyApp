namespace Shootsy.Dtos
{
    public class UserFilesDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string Path { get; set; }

        public DateTime CreateDate { get; set; }

        public bool isDeleted { get; set; }
    }
}
