using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.Common.Data.Mapping
{
    public class SettingMap : EntityBaseConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            base.Configure(builder);
            builder.ToTable("CommonSetting");
            builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
            builder.Property(s => s.Value).IsRequired().HasMaxLength(2000);
        }
    }
}
