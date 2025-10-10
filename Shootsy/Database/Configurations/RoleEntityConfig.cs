using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class RoleEntityConfig : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> entity)
        {
            entity.ToTable("roles", "security");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор роли");
            entity.Property(x => x.RoleName)
                .HasColumnName("role_name")
                .HasComment("Название роли");
            entity.Property(x => x.isSuperUser)
                .HasColumnName("is_super_user")
                .HasComment("Признак суперпользователя");
            entity.Property(x => x.isActive)
                .HasColumnName("is_active")
                .HasComment("Признак актуальности роли");
            entity.HasData(
                new RoleEntity { Id = 1, RoleName = "Администратор", isSuperUser = true, isActive = true },
                new RoleEntity { Id = 2, RoleName = "Обычный пользователь", isSuperUser = false, isActive = true }
                );
        }
    }
}
