using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Nsr.Wh.Web.Data.Mappings;

namespace Nsr.Wh.Web.Data
{
    public class WpWhDbContext : DbContext//, IDbContext
    {
        private readonly string _connectionString;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public WpWhDbContext(DbContextOptions<WpWhDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public WpWhDbContext(DbContextOptions<WpWhDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
        }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
             if (_connectionString != null) //
            {                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("Nsr.Wh.Web"));
                //optionsBuilder.UseSqlite(_connectionString, b => b.MigrationsAssembly("Wp.Data"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            

            // dynamically 
            System.Type configType = typeof(ResumeMap);
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
