
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Data.Mappings
{
    public class SkillItemMap : EntityBaseConfiguration<SkillItem>
    {
        public override void Configure(EntityTypeBuilder<SkillItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProfileResume_SkillItem");
            //this.HasKey(s => s.EntityId);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        }
        
    }
}
