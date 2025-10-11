namespace Shootsy.Database.Entities
{
    public class UserRoleEntity
    {
        public int Id { get; set; }
        public int User { get; set; }
        public int Role { get; set; }
        public bool isActive { get; set; }
        public required UserEntity UserEntity { get; set; }
        public required RoleEntity RoleEntity { get; set; }
    }
}
