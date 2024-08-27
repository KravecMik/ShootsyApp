using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class CooperationTypeEntityConfig : IEntityTypeConfiguration<CooperationTypeEntity>
    {
        public void Configure(EntityTypeBuilder<CooperationTypeEntity> entity)
        {
            entity.ToTable("cooperation_types", "security");

            entity.HasComment("Типы сотрудничества");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор города");

            entity.Property(x => x.TypeName)
                .HasColumnName("type_name")
                .HasComment("Название города");

            entity.HasData(
                new CooperationTypeEntity { Id = 1, TypeName = "Расходы оплачивает фотограф" },
                new CooperationTypeEntity { Id = 2, TypeName = "Расходы оплачивает модель" },
                new CooperationTypeEntity { Id = 3, TypeName = "Расходы оплачиваются поровну" }
                );
        }
    }
}
