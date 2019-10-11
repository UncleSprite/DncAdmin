using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DncAdmin.Api.Infrastructure
{
    using DncAdmin.Api.Infrastructure.EntityConfigurations;
    using Models.Rbac;
    public class DncAdminContext : DbContext
    {
        public const string DEFAULT_SCHEME = "dnc";

        public DncAdminContext(DbContextOptions<DncAdminContext> options)
            : base(options)
        {

        }
        public DbSet<DncUser> Users { get; set; }
        public DbSet<DncRole> Roles { get; set; }
        public DbSet<DncMenu> Menus { get; set; }
        public DbSet<DncUserRoleMapping> userRoleMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DncUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DncRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DncMenuEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DncUserRoleMappingEntityTypeConfiguration());
        }
    }
}
