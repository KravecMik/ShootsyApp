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
                .HasMaxLength(50);
            entity.HasIndex(x => x.Login, "Login")
                .IsUnique();
            entity.Property(x => x.Firstname)
                .HasColumnName("firstname")
                .HasMaxLength(50)
                .HasComment("Имя");
            entity.Property(x => x.Lastname)
                .HasColumnName("lastname")
                .HasMaxLength(50)
                .HasComment("Фамилия");
            entity.Property(x => x.City)
                .HasColumnName("city_id")
                .HasComment("Город");
            entity.Property(x => x.Gender)
                .HasColumnName("gender_id")
                .HasComment("Пол");
            entity.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания пользователя");
            entity.Property(x => x.isDelete)
                .HasColumnName("is_delete")
                .HasComment("Признак удаления пользователя");
            entity.Property(x => x.EditDate)
                .HasColumnName("edit_date")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата редактирования пользователя");
            entity.Property(x => x.Discription)
                .HasColumnName("discription")
                .HasComment("Описание")
                .HasMaxLength(250);
            entity.Property(x => x.ITProfession)
                .HasColumnName("it_profession_id")
                .HasComment("Тип it специальности");
            entity.Property(x => x.Password)
                .HasColumnName("password")
                .HasComment("Хэш пароль пользователя");
            entity.HasOne(u => u.UserTypeEntity)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.ITProfession);
            entity.HasOne(u => u.CityEntity)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.City);
            entity.HasOne(u => u.GenderEntity)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.Gender);
        }
    }
}
