using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Tenants;
using Wp.Data.Mappings.Presentation;

namespace Wp.Data
{
    public class FrontendDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly Tenant _tenant;

        private string GetConnString(string source)
        {
            SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(source);
            conn.InitialCatalog = conn.InitialCatalog + "-Frontend";
            return conn.ConnectionString;
            //// "PConnection": "Filename=./WpCore2.sqlite",
            //source = source.Replace(".sqlite", "-presentation.sqlite");
            //return source;
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        //public WpDbContext(DbContextOptions<WpDbContext> options) : base(options)
        //{
        //    //Database.EnsureCreated();
        //}

        public FrontendDbContext(DbContextOptions<FrontendDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
        }

        public FrontendDbContext(DbContextOptions<FrontendDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenant = tenantService.GetTenant();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (_tenant != null) // tenant
            {
                 optionsBuilder.UseSqlServer(GetConnString(_tenant.ConnectionString));

                //optionsBuilder.UseSqlite(GetConnString(_tenant.ConnectionString));
            }
            else if (_connectionString != null) //
            {
                optionsBuilder.UseSqlServer(GetConnString(_connectionString), b => b.MigrationsAssembly("Wp.Data"));
               // optionsBuilder.UseSqlite(GetConnString(_connectionString), b => b.MigrationsAssembly("Wp.Data"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // new SettingMap(builder.Entity<Setting>()); // digivers style (not recommanded)

            // dynamically 
            System.Type configType = typeof(FrontendWebPageMap);
            var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
                .Where(t => !string.IsNullOrEmpty(t.Namespace))
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(FrontendEntityBaseConfiguration<>));

            foreach (var t in typesToRegister)
            {
                dynamic configInstance = Activator.CreateInstance(t);
                builder.ApplyConfiguration(configInstance);
            }

            // manaully
            //builder.ApplyConfiguration(new SettingMap());

        }
    }
}
