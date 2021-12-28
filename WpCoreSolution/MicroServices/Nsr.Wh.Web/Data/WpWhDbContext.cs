using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Nsr.Wh.Web.Data.Mappings;
using Nsr.RestClient.Models.ActivityLogs;
using System.Collections.Generic;
using Nsr.RestClient.RestClients.ActivityLogs;
using System.Threading.Tasks;
using System.Threading;

namespace Nsr.Wh.Web.Data
{
    public class WpWhDbContext : DbContext//, IDbContext
    {
        private readonly string _connectionString;
        private readonly IActivityLogWebApi _activityLogWebApi;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public WpWhDbContext(DbContextOptions<WpWhDbContext> options, IActivityLogWebApi activityLogWebApi) : base(options)
        {
            _activityLogWebApi = activityLogWebApi;
            //Database.EnsureCreated();
        }

        public WpWhDbContext(DbContextOptions<WpWhDbContext> options, IActivityLogWebApi activityLogWebApi, string connectionString) : base(options)
        {
            _activityLogWebApi = activityLogWebApi;
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (_connectionString != null) //
            {
                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("Nsr.Wh.Web"));
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

            foreach (var t in typesToRegister)
            {
                dynamic configInstance = Activator.CreateInstance(t);
                builder.ApplyConfiguration(configInstance);
            }

            // manaully
            //builder.ApplyConfiguration(new SettingMap());

        }

        public override int SaveChanges()
        {
            OnBeforeSaveChanges();
            return base.SaveChanges();
        }        

        private void OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                //if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                //    continue;

                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry();
                auditEntry.EntityType = entry.Entity.GetType().Name;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.EntityId = (int)property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.Properties.Add(new AuditEntryProperty { Name = propertyName, NewValue = property.CurrentValue });
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.Properties.Add(new AuditEntryProperty { Name = propertyName, OldValue = property.OriginalValue });
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.Properties.Add(new AuditEntryProperty { Name = propertyName, OldValue = property.OriginalValue, NewValue = property.CurrentValue });
                            }
                            break;
                    }
                }
            }
            _activityLogWebApi.InsertAuditEntries(auditEntries).GetAwaiter().GetResult();
            //foreach (var auditEntry in auditEntries)
            //{
            //    AuditLogs.Add(auditEntry.ToAudit());
            //}
        }
    }
}
