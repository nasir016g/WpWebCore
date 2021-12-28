using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Common.Data;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Data.Mapping
{
    public class ActivityLogItemMap : EntityBaseConfiguration<ActivityLogItem>
    {
        public override void Configure(EntityTypeBuilder<ActivityLogItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("ActivityLogItem");

            builder.Property(x => x.EntityKey).HasMaxLength(400);

            builder.HasOne(x => x.ActivityLog)
                .WithMany(y => y.ActivityLogItems)
                .HasForeignKey(x => x.ActivityLogId)
                .IsRequired();
                

        }
    }
}
