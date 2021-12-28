using Microsoft.EntityFrameworkCore;
using Nsr.ActivityLogs.Web.Data.Mapping;
using Nsr.Common.Data;
using System.Reflection;

namespace Nsr.ActivityLogs.Web.Data
{
    public class ActivityLogDbContext : DbContext
    {
        private readonly string _connectionString;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public ActivityLogDbContext(DbContextOptions<ActivityLogDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public ActivityLogDbContext(DbContextOptions<ActivityLogDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (_connectionString != null) //
            {
                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("Nsr.Common.Data"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // dynamically 
            System.Type configType = typeof(ActivityLogMap);
            var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
                .Where(t => !string.IsNullOrEmpty(t.Namespace))
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(EntityBaseConfiguration<>));

            foreach (var t in typesToRegister)
            {
                dynamic configInstance = Activator.CreateInstance(t);
                builder.ApplyConfiguration(configInstance);
            }          

        }
    }
}
