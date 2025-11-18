namespace Shootsy.Database.Entities
{
    public class UserSessionEntity
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid Guid { get; init; }
        public UserEntity? UserEntity { get; init; }
    }
}