using Shootsy.Core.Interfaces;

namespace Shootsy.Database.Entities
{
    public class FileEntity : IFile
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int User { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ContentPath { get; set; }

        /// <summary>
        /// Сущность пользователя
        /// </summary>
        public UserEntity UserEntity { get; init; }
    }
}
