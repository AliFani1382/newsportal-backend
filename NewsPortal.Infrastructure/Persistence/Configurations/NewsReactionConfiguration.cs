using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Infrastructure.Persistence.Configurations;

public class NewsReactionConfiguration
    : IEntityTypeConfiguration<NewsReaction>
{
    public void Configure(
        EntityTypeBuilder<NewsReaction> builder)
    {
        builder.HasOne(r => r.News)
            .WithMany()
            .HasForeignKey(r => r.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new
        {
            r.NewsId,
            r.UserId
        })
        .IsUnique();
    }
}