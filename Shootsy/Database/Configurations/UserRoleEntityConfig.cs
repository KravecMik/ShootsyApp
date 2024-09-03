using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class UserRoleEntityConfig : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> entity)
        {
            entity.ToTable("user_roles", "security");

            entity.HasComment("Связь ролей пользователя");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор связи роли и пользователя");

            entity.Property(x => x.User)
                .HasColumnName("user_id")
                .HasComment("Идентификатор пользователя");

            entity.Property(x => x.Role)
                .HasColumnName("role_id")
                .HasComment("Идентификатор роли");

            entity.Property(x => x.isActive)
                .HasColumnName("is_active")
                .HasComment("Признак актуальности связи роли пользователя");

            entity.HasOne(x => x.RoleEntity)
                .WithMany(x => x.UserRoleEntity)
                .HasForeignKey(x => x.Role)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(x => x.UserEntity)
                .WithMany(x => x.UserRoleEntity)
                .HasForeignKey(x => x.User)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
