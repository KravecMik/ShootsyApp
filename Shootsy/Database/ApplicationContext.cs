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
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserSessionEntity> UserSessions { get; set; }
        public DbSet<UserTypeEntity> UserTypes { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<GenderEntity> Genders { get; set; }
        public DbSet<CooperationTypeEntity> CooperationTypes { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfig());
            modelBuilder.ApplyConfiguration(new UserSessionEntityConfig());
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfig());
            modelBuilder.ApplyConfiguration(new CityEntityConfig());
            modelBuilder.ApplyConfiguration(new GenderEntityConfig());
            modelBuilder.ApplyConfiguration(new CooperationTypeEntityConfig());
            modelBuilder.ApplyConfiguration(new FileEntityConfig());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            Console.WriteLine($"Состояние подключения к БД: {optionsBuilder.Options.ContextType}");
        }
    }
}
