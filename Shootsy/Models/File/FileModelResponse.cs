using Newtonsoft.Json;
using Shootsy.Core.Interfaces;

namespace Shootsy.Models.File
{
    public class FileModelResponse
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int UserID { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ContentPath { get; set; }
    }
}
