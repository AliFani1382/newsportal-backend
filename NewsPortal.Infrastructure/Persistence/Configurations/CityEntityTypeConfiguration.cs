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
    public sealed class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(X => X.Id);

            builder.Property(X => X.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(X => X.Slug)
               .IsRequired()
               .HasMaxLength(150);

            builder.HasIndex(X => X.Slug)
                .IsUnique();

            builder.Property(X => X.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
