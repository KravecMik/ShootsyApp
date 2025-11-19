namespace Shootsy.Database.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public required string Login { get; set; }
        public required byte[] Password { get; set; }
        public int GenderId { get; set; }
        public int CityId { get; set; }
        public required string Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Description { get; set; }
        public int ProfessionId { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public ICollection<UserSessionEntity>? UserSessionEntity { get; init; } = new List<UserSessionEntity>();
        public CityEntity? CityEntity { get; init; }
        public GenderEntity? GenderEntity { get; init; }
        public ICollection<UserRoleEntity>? UserRoleEntity { get; init; } = new List<UserRoleEntity>();
        public ProfessionEntity? ProfessionEntity { get; init; }

        public ICollection<PostEntity> Posts { get; init; } = new List<PostEntity>();
        public ICollection<CommentEntity> Comments { get; init; } = new List<CommentEntity>();
        public ICollection<LikeEntity> Likes { get; init; } = new List<LikeEntity>();
    }
}