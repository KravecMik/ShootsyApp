using Microsoft.EntityFrameworkCore;
using Shootsy.Database.Entities;
using System.Reflection;

namespace Shootsy.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<LikeEntity> Likes { get; set; }
        public DbSet<UserSessionEntity> UserSessions { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<GenderEntity> Genders { get; set; }
        public DbSet<ProfessionEntity> Professions { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                 .EnableSensitiveDataLogging() 
                 .EnableDetailedErrors();    
        }
    }
}