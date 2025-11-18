using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class GenderEntityConfig : IEntityTypeConfiguration<GenderEntity>
    {
        public void Configure(EntityTypeBuilder<GenderEntity> entity)
        {
            entity.ToTable("gender", "public");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор пола");

            entity.Property(x => x.GenderName)
                .HasColumnName("gender_name")
                .HasComment("Название пола");

            entity.HasData(
                new GenderEntity { Id = 1, GenderName = "Мужской" },
                new GenderEntity { Id = 2, GenderName = "Женский" }
                );
        }
    }
}
