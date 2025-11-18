using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class UserSessionEntityConfig : IEntityTypeConfiguration<UserSessionEntity>
    {
        public void Configure(EntityTypeBuilder<UserSessionEntity> entity)
        {
            entity.ToTable("user_sessions", "security");
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Guid, "guid");
            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор сессии");

            entity.Property(x => x.StartDate)
                .HasColumnName("date_from")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата начала сессии");

            entity.Property(x => x.EndDate)
                .HasColumnName("date_to")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата окончания сессии");

            entity.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasComment("Идентификатор пользователя");

            entity.Property(x => x.Guid)
                .HasColumnName("guid")
                .HasComment("GUID сессии");

            entity.HasOne(x => x.UserEntity)
                .WithMany(x => x.UserSessionEntity)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
