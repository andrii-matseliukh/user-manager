using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagerCore.Entities;

namespace UserManagerData.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(2500);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasMany(s => s.Accounts)
                .WithOne(s => s.Group)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
