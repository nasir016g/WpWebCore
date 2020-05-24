using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core.Domain.Tenants;

namespace Wp.Data
{
    public class TenantDbContext :  DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public TenantDbContext(DbContextOptions<TenantDbContext> options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
