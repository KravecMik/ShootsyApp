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

            entity.HasComment("Данные по пользователям");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Login, "User_Login")
                .IsUnique()
                .HasFilter(@"(is_delete = false)");

            entity.Property(x => x.Firstname)
                .HasColumnName("firstname")
                .HasMaxLength(50)
                .HasComment("Имя");

            entity.Property(x => x.Lastname)
                .HasColumnName("lastname")
                .HasMaxLength(50)
                .HasComment("Фамилия");

            entity.Property(x => x.Patronymic)
                .HasColumnName("patronymic")
                .HasMaxLength(50)
                .HasComment("Отчество");

            entity.Property(x => x.Fullname)
                .HasColumnName("fullname")
                .HasComment("Полное имя");

            entity.Property(x => x.City)
                .HasColumnName("city_id")
                .HasComment("Город");

            entity.Property(x => x.Contact)
                .HasColumnName("contact")
                .HasComment("Контакт для связи")
                .HasMaxLength(100);

            entity.Property(x => x.Gender)
                .HasColumnName("gender_id")
                .HasComment("Пол");

            entity.Property(x => x.CooperationType)
                .HasColumnName("cooperation_type_id")
                .HasComment("Тип сотрудничества");

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

            entity.Property(x => x.isHasActiveSubscribe)
                .HasColumnName("is_has_active_subscribe")
                .HasComment("Признак наличия активной подписки");

            entity.Property(x => x.isNude)
                .HasColumnName("is_nude")
                .HasComment("Съемка ню");

            entity.Property(x => x.Type)
                .HasColumnName("type_id")
                .HasComment("Тип пользователя");

            entity.Property(x => x.Password)
                .HasColumnName("password_id")
                .HasComment("Хэш пароль пользователя");
        }
    }
}
