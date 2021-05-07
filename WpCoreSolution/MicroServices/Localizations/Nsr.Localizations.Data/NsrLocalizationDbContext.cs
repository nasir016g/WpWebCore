using Microsoft.EntityFrameworkCore;
using Nsr.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.Localizations.Data
{
    public class NsrLocalizationDbContext : DbContext//, IDbContext
    {
        private readonly string _connectionString;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public NsrLocalizationDbContext(DbContextOptions<NsrLocalizationDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public NsrLocalizationDbContext(DbContextOptions<NsrLocalizationDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (_connectionString != null) //
            {
                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("Nsr.Localization.Data"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // dynamically 
            System.Type configType = typeof(LanguageMap);
            var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
                .Where(t => !string.IsNullOrEmpty(t.Namespace))
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(EntityBaseConfiguration<>));

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
