using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Infrastructure.Persistence.Configurations;

public sealed class NewsImageConfiguration
    : IEntityTypeConfiguration<NewsImage>
{
    public void Configure(EntityTypeBuilder<NewsImage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DisplayOrder)
            .IsRequired();

        builder.HasOne(x => x.News)
     .WithMany(n => n.NewsImages)
     .HasForeignKey(x => x.NewsId)
     .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new
        {
            x.NewsId,
            x.DisplayOrder
        });
    }
}