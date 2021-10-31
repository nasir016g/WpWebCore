using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Sections;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Sections
{
    public class ResumeMap : EntityBaseConfiguration<WorkHistorySection>
    {
        public override void Configure(EntityTypeBuilder<WorkHistorySection> entityBuilder)
        {
            entityBuilder.ToTable("Section_Resume"); 
        }
    }
}
