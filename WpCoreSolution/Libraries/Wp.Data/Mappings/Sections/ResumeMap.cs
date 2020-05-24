using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Sections;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Sections
{
    public class ResumeMap : EntityBaseConfiguration<ResumeSection>
  {
      public override void Configure(EntityTypeBuilder<ResumeSection> entityBuilder) 
        {
            //entityBuilder.ToTable("Section_Resume"); 
    }
  } 
}
