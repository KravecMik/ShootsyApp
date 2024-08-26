namespace Shootsy.Dtos
{
    public class FilesDto
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string ContentPath { get; set; } = string.Empty;

        public string Extension { get; set; } = ".jpg";

        public bool isDeleted { get; set; } = false;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public bool isAvatar { get; set; } = false;
    }
}
