using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Common.Data;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Data.Mapping
{
    public class ActivityLogTypeMap : EntityBaseConfiguration<ActivityLogType>
    {
        public override void Configure(EntityTypeBuilder<ActivityLogType> builder)
        {
            base.Configure(builder);
            builder.ToTable("ActivityLogType");

            builder.Property(logType => logType.SystemKeyword).HasMaxLength(100).IsRequired();
            builder.Property(logType => logType.Name).HasMaxLength(200).IsRequired();
        }
    }    
}
