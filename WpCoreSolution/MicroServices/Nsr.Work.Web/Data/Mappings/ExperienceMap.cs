

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Data.Mappings
{
    public class ExperienceMap : EntityBaseConfiguration<Experience>
    {
        public override void Configure(EntityTypeBuilder<Experience> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProfileExperience");
            //this.HasKey(w => w.EntityId);
            builder.Property(w => w.Name).IsRequired().HasMaxLength(200);
            builder.HasOne(w => w.Resume)
                .WithMany(r => r.Experiences)
                .HasForeignKey(s => s.ResumeId);
        }
       
    }
}
