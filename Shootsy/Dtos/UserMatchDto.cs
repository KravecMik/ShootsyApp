namespace Shootsy.Dtos
{
    public class UserMatchDto
    {
        public int Id { get; set; }

        public int mUserId { get; set; }

        public int fUserId { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
