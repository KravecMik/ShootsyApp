namespace Shootsy.Dtos
{
    public class UserSubscribeDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime SubscribeDateFrom { get; set; }

        public DateTime SubscribeDateTo { get; set; }

        public decimal Sum { get; set; }

        public bool isActive { get; set; }
    }
}
