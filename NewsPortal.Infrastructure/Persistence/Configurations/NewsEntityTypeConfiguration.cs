using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Infrastructure.Persistence.Configurations
{
    public sealed class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(X => X.Id);

            builder.Property(X => X.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(X => X.Slug)
                 .IsRequired()
                 .HasMaxLength(250);

            builder.HasIndex(X => X.Slug)
                .IsUnique();

            builder.Property(X => X.Content)
                .IsRequired();

            builder.Property(X => X.ImagePath)
                .HasMaxLength(500);

            builder.Property(X => X.PublicationDate)
                .IsRequired();

            builder.Property(X => X.Status)
                .HasConversion<int>()
                .HasDefaultValue(NewsStatus.Draft)
                .IsRequired();

            builder.HasOne(X => X.City)
                .WithMany()
                .HasForeignKey(X => X.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(X => X.Category)
               .WithMany(X => X.News)
               .HasForeignKey(X => X.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(X => X.Writer)
               .WithMany(X => X.News)
               .HasForeignKey(X => X.WriterId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
