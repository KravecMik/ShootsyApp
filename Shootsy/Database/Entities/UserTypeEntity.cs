namespace Shootsy.Database.Entities
{
    public class UserTypeEntity
    {
        public int Id { get; set; }
        public required string TypeName { get; set; }
        public List<UserEntity> Users { get; set; } = new();
    }
}
