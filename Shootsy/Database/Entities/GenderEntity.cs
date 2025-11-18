namespace Shootsy.Database.Entities
{
    public class GenderEntity
    {
        public int Id { get; set; }
        public required string GenderName { get; set; }
        public List<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
