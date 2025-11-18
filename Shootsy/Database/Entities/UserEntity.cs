namespace Shootsy.Database.Entities
{
    public class UserEntity
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public required string Login { get; set; }
        public required byte[] Password { get; init; }
        public int GenderId { get; init; }
        public int CityId { get; init; }
        public required string Firstname { get; init; }
        public string? Lastname { get; init; }
        public string? Description { get; init; }
        public int ProfessionId { get; init; }
        public bool? IsDeleted { get; init; } = false;

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