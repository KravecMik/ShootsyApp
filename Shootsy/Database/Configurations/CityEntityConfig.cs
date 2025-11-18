using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class CityEntityConfig : IEntityTypeConfiguration<CityEntity>
    {
        public void Configure(EntityTypeBuilder<CityEntity> entity)
        {
            entity.ToTable("cities", "public");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор города");

            entity.Property(x => x.CityName)
                .HasColumnName("city_name")
                .HasComment("Название города");

            entity.HasData(
                new CityEntity { Id = 1, CityName = "Новосибирск" },
                new CityEntity { Id = 2, CityName = "Барнаул" },
                new CityEntity { Id = 3, CityName = "Москва" }
                );
        }
    }
}
