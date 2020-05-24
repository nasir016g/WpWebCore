using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Wp.Core;
using Wp.Core.Domain.Tenants;
using Wp.Core.Mapping.WebPages;
using Wp.Core.Security;
using Wp.Data.Mappings;

namespace Wp.Data
{
    public class WpDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        private readonly string _connectionString;
        private readonly Tenant _tenant;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        //public WpDbContext(DbContextOptions<WpDbContext> options) : base(options)
        //{
        //    //Database.EnsureCreated();
        //}

        public WpDbContext(DbContextOptions<WpDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
        }

        public WpDbContext(DbContextOptions<WpDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenant = tenantService.GetTenant();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_tenant != null) // tenant
            {
               // optionsBuilder.UseSqlServer(_tenant.ConnectionString);
                optionsBuilder.UseSqlite(_tenant.ConnectionString);
            }
            else if (_connectionString != null) //
            {
                //optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("Wp.Data"));
                optionsBuilder.UseSqlite(_connectionString, b => b.MigrationsAssembly("Wp.Data"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // new SettingMap(builder.Entity<Setting>()); // digivers style (not recommanded)

            // dynamically 
            System.Type configType = typeof(WebPageMap);
            var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
                .Where(t => !string.IsNullOrEmpty(t.Namespace))
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(EntityBaseConfiguration<>));

            foreach(var t in typesToRegister)
            {
                dynamic configInstance = Activator.CreateInstance(t);
                builder.ApplyConfiguration(configInstance);
            }

            // manaully
            //builder.ApplyConfiguration(new SettingMap());
          
        }
    }
}
