using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Infrastructure.Persistence.Configurations;

public class BookmarkConfiguration
    : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(
        EntityTypeBuilder<Bookmark> builder)
    {
        builder.HasOne(b => b.News)
            .WithMany()
            .HasForeignKey(b => b.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => new
        {
            b.NewsId,
            b.UserId
        })
        .IsUnique();
    }
}