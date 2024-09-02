using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class FileEntityConfig : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> entity)
        {
            entity.ToTable("files", "security");

            entity.HasComment("Файлы");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор файла");

            entity.Property(x => x.CreateDate)
                .HasColumnName("create_date")
                .HasComment("Дата создания файла");

            entity.Property(x => x.User)
                .HasColumnName("user_id")
                .HasComment("Идентификатор пользователя");

            entity.Property(x => x.FileName)
                .HasColumnName("file_name")
                .HasComment("Наименование файла");

            entity.Property(x => x.Extension)
                .HasColumnName("extension")
                .HasComment("Расширение файла");

            entity.Property(x => x.ContentPath)
                .HasColumnName("content_path")
                .HasComment("Путь до содержимого файла");

            entity.HasOne(x => x.UserEntity)
                .WithMany(x => x.FileEntity)
                .HasForeignKey(x => x.User)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
