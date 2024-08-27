using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class UserSessionEntityConfig : IEntityTypeConfiguration<UserSessionEntity>
    {
        public void Configure(EntityTypeBuilder<UserSessionEntity> entity)
        {
            entity.ToTable("user_session", "security");

            entity.HasComment("Сессии пользователей");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Guid, "guid");

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор сессии");

            entity.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания сессии");

            entity.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasComment("Идентификатор пользователя");

            entity.Property(x => x.Guid)
                .HasColumnName("guid")
                .HasComment("GUID сессии");

            entity.HasOne(x => x.User)
                .WithMany(x => x.UserSessions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
