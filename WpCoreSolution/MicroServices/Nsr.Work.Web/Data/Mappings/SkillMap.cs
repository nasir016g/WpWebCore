
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Data.Mappings
{
    public class SkillMap : EntityBaseConfiguration<Skill>
    {
        public override void Configure(EntityTypeBuilder<Skill> builder)
        {
            base.Configure(builder);
            builder.ToTable("Resume_Skill");
            //this.HasKey(s => s.EntityId);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
            builder.HasOne(s => s.Resume)
                .WithMany(r => r.Skills)
                .HasForeignKey(s => s.ResumeId);
        }
        
    }
}
