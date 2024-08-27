using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class PasswordEntityConfig : IEntityTypeConfiguration<PasswordEntity>
    {
        public void Configure(EntityTypeBuilder<PasswordEntity> entity)
        {
            entity.ToTable("passwords", "security");

            entity.HasComment("Пароли пользователей");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор пароля");

            entity.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasComment("Идентификатор пользователя");

            entity.Property(x => x.Password)
                .HasColumnName("password")
                .HasComment("Пароль пользователя");

            entity.Property(x => x.Salt)
                .HasColumnName("salt")
                .HasComment("Соль");

            entity.Property(x => x.isActive)
                .HasColumnName("is_active")
                .HasComment("Признак активности пароля");

            entity.HasOne(u => u.Users)
                .WithMany(u => u.Password)
                .HasForeignKey(u => u.UserId);
        }
    }
}
