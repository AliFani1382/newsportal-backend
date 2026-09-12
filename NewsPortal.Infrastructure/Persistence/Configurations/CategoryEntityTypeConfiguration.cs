using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Infrastructure.Persistence.Configurations
{
    public sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(X => X.Id);

            builder.Property(X => X.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(X => X.Slug)
               .IsRequired()
               .HasMaxLength(250);

            builder.HasIndex(X => X.Slug)
                .IsUnique();

            builder.Property(X => X.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");
                
        }
    }
}
