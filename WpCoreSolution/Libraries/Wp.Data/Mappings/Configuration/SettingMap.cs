using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Configuration;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Configuration
{
    public class SettingMap : EntityBaseConfiguration<Setting>
    {
        //public SettingMap(EntityTypeBuilder<Setting> entityBuilder) 
        //{
        //    entityBuilder.ToTable("Setting");
        //    // this.HasKey(s => s.EntityId);
        //    entityBuilder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        //    entityBuilder.Property(s => s.Value).IsRequired().HasMaxLength(2000);
        //}
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            base.Configure(builder);

            builder.ToTable("Setting");
            builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
            builder.Property(s => s.Value).IsRequired().HasMaxLength(2000);

        }
    }
}
