using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DncAdmin.Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DncAdmin.Api.Infrastructure.EntityConfigurations
{
    public class DncUserEntityTypeConfiguration
        : IEntityTypeConfiguration<DncUser>
    {
        public void Configure(EntityTypeBuilder<DncUser> builder)
        {
            builder.ToTable("Users", DncAdminContext.DEFAULT_SCHEME);

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
               .HasDefaultValueSql("newid()")
               .IsRequired();

            builder.Property(t => t.Account)
                .HasColumnType("nvarchar(20)")
                .IsRequired(true);

            builder.HasIndex(t => t.Account)
                .IsUnique();

            builder.Property(t => t.NiName)
               .HasColumnType("nvarchar(20)");

            builder.Property(t => t.Password)
              .HasColumnType("nvarchar(32)");

            builder.Property(t => t.Avatar)
              .HasColumnType("nvarchar(128)");

            builder.Property(t => t.Status)
            .HasDefaultValue(0);

            builder.Property(t => t.CreateOn)
           .HasColumnType("datetime")
           .HasDefaultValueSql("GETDATE()")
           .IsRequired();

        }
    }
}
