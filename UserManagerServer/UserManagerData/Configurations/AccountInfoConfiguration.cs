using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagerCore.Entities;

namespace UserManagerData.Configurations
{
    public class AccountInfoConfiguration : IEntityTypeConfiguration<AccountInfo>
    {
        public void Configure(EntityTypeBuilder<AccountInfo> builder)
        {
            builder.Property(s => s.Email)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(2500);

            builder
                .HasOne(s => s.Group)
                .WithMany(s => s.Accounts)
                .HasForeignKey(s=>s.GroupId).IsRequired(false);

            //builder.HasData(new AccountInfo[] {
            //    new AccountInfo
            //    {
                    
            //    }
            //});
        }
    }
}
