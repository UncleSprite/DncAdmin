using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DncAdmin.Api.Models.Rbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DncAdmin.Api.Infrastructure.EntityConfigurations
{
    public class DncMenuEntityTypeConfiguration
        : IEntityTypeConfiguration<DncMenu>
    {
        public void Configure(EntityTypeBuilder<DncMenu> builder)
        {
            builder.ToTable("menu", DncAdminContext.DEFAULT_SCHEME);

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasDefaultValueSql("newid()")
                .IsRequired();

            builder.HasOne(m => m.Parent)
                .WithMany(m => m.SubMenus)
                .HasForeignKey(t => t.ParentId)
                .IsRequired(false);

            builder.Property(m => m.Name)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.Property(m => m.Url)
                .HasColumnType("nvarchar(255)");

            builder.Property(m => m.Alias)
                .HasColumnType("nvarchar(255)");

            builder.Property(m => m.Icon)
              .HasColumnType("nvarchar(255)");

            builder.Property(m => m.Description)
              .HasColumnType("nvarchar(255)");

            builder.Property(m => m.Component)
              .HasColumnType("nvarchar(255)");

            builder.Property(t => t.CreateOn)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
        }
    }
}
