namespace Shootsy.Models.File
{
    public class GetFileByIdResponse
    {
        public required string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int IdUser { get; set; }
        public required Database.Mongo.FileInfo FileInfo { get; set; }
    }
}
