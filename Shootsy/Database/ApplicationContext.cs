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
        public DbSet<CityEntity> CityEntities { get; set; }
        public DbSet<GenderEntity> GenderEntities { get; set; }
        public DbSet<CooperationTypeEntity> CooperationTypes { get; set; }

        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfig());
            modelBuilder.ApplyConfiguration(new UserSessionEntityConfig());
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfig());
            modelBuilder.ApplyConfiguration(new CityEntityConfig());
            modelBuilder.ApplyConfiguration(new  GenderEntityConfig());
            modelBuilder.ApplyConfiguration(new CooperationTypeEntityConfig());
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
