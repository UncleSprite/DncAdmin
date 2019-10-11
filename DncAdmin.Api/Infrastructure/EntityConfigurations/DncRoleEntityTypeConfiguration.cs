using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DncAdmin.Api.Models.Rbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DncAdmin.Api.Infrastructure.EntityConfigurations
{
    public class DncRoleEntityTypeConfiguration
        : IEntityTypeConfiguration<DncRole>
    {
        public void Configure(EntityTypeBuilder<DncRole> builder)
        {
            builder.ToTable("role", DncAdminContext.DEFAULT_SCHEME);

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasDefaultValueSql("newid()")
                .IsRequired();

            builder.Property(m => m.Name)
              .HasColumnType("nvarchar(50)")
              .IsRequired();

            builder.Property(m => m.Description)
           .HasColumnType("nvarchar(255)");

            builder.Property(t => t.CreateOn)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        }
    }
}
