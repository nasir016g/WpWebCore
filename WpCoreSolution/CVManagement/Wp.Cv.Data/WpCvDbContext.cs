using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Wp.Cv.Data.Mappings;

namespace Wp.Cv.Data
{
    public class WpCvDbContext : DbContext//, IDbContext
    {
        private readonly string _connectionString;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        //public WpDbContext(DbContextOptions<WpDbContext> options) : base(options)
        //{
        //    //Database.EnsureCreated();
        //}

        public WpCvDbContext(DbContextOptions<WpCvDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
        }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
             if (_connectionString != null) //
            {                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("Wp.Data"));
                //optionsBuilder.UseSqlite(_connectionString, b => b.MigrationsAssembly("Wp.Data"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            

            // dynamically 
            System.Type configType = typeof(EducationMap);
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
