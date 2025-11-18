using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.ToTable("comments", "public");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор комментария")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Text)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(c => c.CreateDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(c => c.EditDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.PostId)
                .IsRequired(false);

            builder.Property(c => c.ParentCommentId)
                .IsRequired(false);

            builder.HasIndex(c => c.UserId);
            builder.HasIndex(c => c.PostId);
            builder.HasIndex(c => c.ParentCommentId);
            builder.HasIndex(c => c.CreateDate);

            builder.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.ParentComment)
                .WithMany(pc => pc.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Replies)
                .WithOne(r => r.ParentComment)
                .HasForeignKey(r => r.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Likes)
                .WithOne(l => l.Comment)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}