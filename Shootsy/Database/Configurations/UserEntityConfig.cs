using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class UserEntityConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> entity)
        {
            entity.ToTable("users", "security");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор пользователя")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Login)
                .HasColumnName("Login")
                .HasComment("Имя пользователя")
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.Login, "Login")
                .IsUnique();

            entity.Property(x => x.Firstname)
                .HasColumnName("firstname")
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Имя");

            entity.Property(x => x.Lastname)
                .HasColumnName("lastname")
                .HasMaxLength(50)
                .IsRequired(false)
                .HasComment("Фамилия");

            entity.Property(x => x.CityId)
                .HasColumnName("city_id")
                .IsRequired()
                .HasComment("Город");

            entity.Property(x => x.GenderId)
                .HasColumnName("gender_id")
                .IsRequired()
                .HasComment("Пол");

            entity.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired()
                .HasComment("Дата создания пользователя");

            entity.Property(x => x.IsDeleted)
                .HasComment("Признак удаления пользователя")
                .HasColumnName("is_delete")
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(x => x.EditDate)
                .HasComment("Дата редактирования пользователя")
                .HasColumnName("edit_date")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(x => x.Description)
                .HasComment("Описание")
                .HasColumnName("description")
                .IsRequired(false)
                .HasMaxLength(250);

            entity.Property(x => x.ProfessionId)
                .HasComment("Тип it специальности")
                .HasColumnName("it_profession_id")
                .IsRequired();

            entity.Property(x => x.Password)
                .HasComment("Хэш пароль пользователя")
                .HasColumnName("password")
                .IsRequired();

            entity.HasIndex(u => u.IsDeleted);
            entity.HasIndex(u => new { u.Firstname, u.Lastname });
            entity.HasIndex(u => u.ProfessionId);

            entity.HasOne(u => u.CityEntity)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.CityId);

            entity.HasOne(u => u.GenderEntity)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.GenderId);

            entity.HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.ProfessionEntity)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.ProfessionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
