using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Sections;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Sections
{
    public class ContactFormMap : EntityBaseConfiguration<ContactFormSection>
    {
        public override void Configure(EntityTypeBuilder<ContactFormSection> entityBuilder) 
        {
           
            //entityBuilder.ToTable("Section_ContactForm");

        }
    }
}
