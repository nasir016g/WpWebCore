
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Wh.Web.Domain;

namespace Nsr.Wh.Web.Data.Mappings
{
    public class ProjectMap : EntityBaseConfiguration<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);
            builder.ToTable("Resume_Project");
            //this.HasKey(p => p.EntityId);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.HasOne(p => p.Experience)
                .WithMany(w => w.Projects)
                .HasForeignKey(p => p.ExperienceId);
        }
    }
}
