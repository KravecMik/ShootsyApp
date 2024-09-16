using Microsoft.EntityFrameworkCore;
using Shootsy.Database.Configurations;
using Shootsy.Database.Entities;

namespace Shootsy.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserSessionEntity> UserSessions { get; set; }
        public DbSet<UserTypeEntity> UserTypes { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<GenderEntity> Genders { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfig());
            modelBuilder.ApplyConfiguration(new UserSessionEntityConfig());
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfig());
            modelBuilder.ApplyConfiguration(new CityEntityConfig());
            modelBuilder.ApplyConfiguration(new GenderEntityConfig());
            modelBuilder.ApplyConfiguration(new FileEntityConfig());
            modelBuilder.ApplyConfiguration(new RoleEntityConfig());
            modelBuilder.ApplyConfiguration(new UserRoleEntityConfig());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        }
    }
}
