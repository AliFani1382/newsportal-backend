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
    public sealed class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(X => X.Id);

            builder.Property(X => X.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(X => X.Name)
                .IsUnique();
        }
    }
}
