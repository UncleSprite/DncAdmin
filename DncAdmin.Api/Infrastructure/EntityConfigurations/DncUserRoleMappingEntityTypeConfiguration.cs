using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DncAdmin.Api.Models.Rbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DncAdmin.Api.Infrastructure.EntityConfigurations
{
    public class DncUserRoleMappingEntityTypeConfiguration
        : IEntityTypeConfiguration<DncUserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<DncUserRoleMapping> builder)
        {
            builder.ToTable("User_Role_R", DncAdminContext.DEFAULT_SCHEME);

            builder.HasKey(t => new
            {
                t.UserId,
                t.RoleId
            });

            builder.HasOne(t => t.DncRole)
                .WithMany(t => t.UserRoles)
                .HasForeignKey(t => t.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DncUser)
                .WithMany(t => t.UserRoles)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
