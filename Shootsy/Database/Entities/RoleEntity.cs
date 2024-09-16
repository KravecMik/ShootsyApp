namespace Shootsy.Database.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool isSuperUser { get; set; }
        public bool isActive { get; set; }
        public ICollection<UserRoleEntity>? UserRoleEntity { get; init; }
    }
}
