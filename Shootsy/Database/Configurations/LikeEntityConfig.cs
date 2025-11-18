using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shootsy.Database.Entities;

namespace Shootsy.Database.Configurations
{
    public class LikeEntityConfiguration : IEntityTypeConfiguration<LikeEntity>
    {
        public void Configure(EntityTypeBuilder<LikeEntity> builder)
        {
            builder.ToTable("likes", "public");
            builder.HasKey(x => x.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(l => l.CreateDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(l => l.UserId)
                .IsRequired();

            builder.Property(l => l.PostId)
                .IsRequired(false);

            builder.Property(l => l.CommentId)
                .IsRequired(false);

            builder.HasIndex(l => new { l.UserId, l.PostId, l.CommentId })
                .IsUnique()
                .HasFilter(@$"(""{nameof(LikeEntity.PostId)}"" IS NOT NULL OR ""{nameof(LikeEntity.CommentId)}"" IS NOT NULL)");

            builder.HasIndex(l => l.UserId);
            builder.HasIndex(l => l.PostId);
            builder.HasIndex(l => l.CommentId);
            builder.HasIndex(l => l.CreateDate);

            builder.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Comment)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}