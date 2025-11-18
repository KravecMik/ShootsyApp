namespace Shootsy.Database.Entities
{
    public class CityEntity
    {
        public int Id { get; set; }
        public required string CityName { get; set; }
        public List<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
