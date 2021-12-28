using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Common.Data;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Data.Mapping
{
    public class ActivityLogMap : EntityBaseConfiguration<ActivityLog>
    {
        public override void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("ActivityLog");

            builder.Property(al => al.EntityName).HasMaxLength(400);

            builder.HasOne(al => al.ActivityLogType)
               .WithMany()
               .HasForeignKey(al => al.ActivityLogTypeId)
               .IsRequired();
        }
    }
}
