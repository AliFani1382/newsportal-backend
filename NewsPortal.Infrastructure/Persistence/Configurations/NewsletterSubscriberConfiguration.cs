using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Infrastructure.Persistence.Configurations;

public class NewsletterSubscriberConfiguration
    : IEntityTypeConfiguration<NewsletterSubscriber>
{
    public void Configure(EntityTypeBuilder<NewsletterSubscriber> builder)
    {
        builder.Property(n => n.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.HasIndex(n => n.Email)
            .IsUnique();
    }
}