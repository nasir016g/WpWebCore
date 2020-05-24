using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Logging;

namespace Wp.Data.Mappings.Logging
{
    public class LogMap : EntityBaseConfiguration<Log>
    {
        public override void Configure(EntityTypeBuilder<Log> entityBuilder) 
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("Log");
            entityBuilder.HasKey(l => l.Id);
            entityBuilder.Property(l => l.ShortMessage).IsRequired();
            entityBuilder.Property(l => l.IpAddress).HasMaxLength(200);

            entityBuilder.Ignore(l => l.LogLevel);
        }
    }
}
