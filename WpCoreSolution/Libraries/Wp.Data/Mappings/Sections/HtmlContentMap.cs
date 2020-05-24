using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Sections;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Sections
{
    public class HtmlContentMap : EntityBaseConfiguration<HtmlContentSection>
  {
    public override void Configure(EntityTypeBuilder<HtmlContentSection> entityBuilder) 
        {
            entityBuilder.Property(c => c.Html).HasMaxLength(4000);
            //entityBuilder.ToTable("Section_HtmlContent"); 
    }

    } 
}
