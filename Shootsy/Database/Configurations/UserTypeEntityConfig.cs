using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class UserTypeEntityConfig : IEntityTypeConfiguration<UserTypeEntity>
    {
        public void Configure(EntityTypeBuilder<UserTypeEntity> entity)
        {
            entity.ToTable("user_type", "security");

            entity.HasComment("Типы пользователей");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор типа");

            entity.Property(x => x.TypeName)
                .HasColumnName("type_name")
                .HasComment("Тип пользователя");

            entity.HasData(
                new UserTypeEntity { Id = 1, TypeName = "Фотограф" },
                new UserTypeEntity { Id = 2, TypeName = "Модель" },
                new UserTypeEntity { Id = 3, TypeName = "Визажист" }
                );
        }
    }
}
