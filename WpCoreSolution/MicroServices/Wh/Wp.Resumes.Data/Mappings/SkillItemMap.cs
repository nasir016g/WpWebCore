
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Data.Mappings
{
    public class SkillItemMap : EntityBaseConfiguration<SkillItem>
    {
        public override void Configure(EntityTypeBuilder<SkillItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("Resume_SkillItem");
            //this.HasKey(s => s.EntityId);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        }
        
    }
}
