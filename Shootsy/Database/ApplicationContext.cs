using Microsoft.EntityFrameworkCore;
using Shootsy.Database.Configurations;
using Shootsy.Database.Entities;

namespace Shootsy.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserSessionEntity> UserSession { get; set; }
        public DbSet<UserTypeEntity> UserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfig());
            modelBuilder.ApplyConfiguration(new UserSessionEntityConfig());
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfig());
        }

        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
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
