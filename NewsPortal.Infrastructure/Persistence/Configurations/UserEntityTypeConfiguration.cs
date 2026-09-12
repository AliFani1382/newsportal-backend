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
    public sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(X => X.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(X => X.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(X => X.PasswordHash)
               .IsRequired()
               .HasMaxLength(500);

            builder.Property(X => X.FullName)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(X => X.IsActive)
                .IsRequired();

            builder.HasIndex(X => X.Username)
                .IsUnique();


            builder.HasIndex(X => X.Email)
                .IsUnique();

            builder.HasOne(X => X.Role)
                .WithMany(X => X.Users)
                .HasForeignKey(X => X.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
