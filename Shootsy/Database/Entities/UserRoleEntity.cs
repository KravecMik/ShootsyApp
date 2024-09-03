namespace Shootsy.Database.Entities
{
    public class UserRoleEntity
    {
        public int Id { get; set; }
        public int User { get; set; }
        public int Role { get; set; }
        public bool isActive { get; set; }
        public UserEntity UserEntity { get; set; }
        public RoleEntity RoleEntity { get; set; }
    }
}
