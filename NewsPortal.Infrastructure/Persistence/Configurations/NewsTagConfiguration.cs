using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Infrastructure.Persistence.Configurations;

public sealed class NewsTagConfiguration
    : IEntityTypeConfiguration<NewsTag>
{
    public void Configure(EntityTypeBuilder<NewsTag> builder)
    {
        builder.HasKey(nt => nt.Id);

        builder.HasOne(nt => nt.News)
            .WithMany(n => n.NewsTags)
            .HasForeignKey(nt => nt.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(nt => nt.Tag)
            .WithMany(t => t.NewsTags)
            .HasForeignKey(nt => nt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(nt => new
        {
            nt.NewsId,
            nt.TagId
        })
        .IsUnique();
    }
}