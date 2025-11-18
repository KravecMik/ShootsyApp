namespace Shootsy.Database.Entities
{
    public class ProfessionEntity
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Category { get; init; }

        public ICollection<UserEntity> Users { get; init; } = new List<UserEntity>();
    }
}
