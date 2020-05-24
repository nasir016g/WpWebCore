using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Sections;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Sections
{
    public class SectionMap : EntityBaseConfiguration<Section>
    {
        public override void Configure(EntityTypeBuilder<Section> entityBuilder)
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("Section");
            //this.HasKey(s => s.EntityId);

            //this.Property(x => x.Description).IsRequired().IsMaxLength(); 
            entityBuilder.HasOne(p => p.WebPage)
             .WithMany(s => s.Sections)
             .HasForeignKey(p => p.WebPageId);


            // tpt associations


            // tpt (ef 4.3)http://stackoverflow.com/questions/9499702/entity-framework-4-3-tph-mapping-and-migration-error
            //this.ToTable("Section");
            //this.Map<HtmlContent>(x => x.ToTable("Section_HtmlContent"))
            //    .Map<MyAccount>(x => x.ToTable("Section_MyAccount"))
            //    .Map<ContactForm>(x => x.ToTable("Section_ContactForm"))
            //    .Map<GalleryFlickr>(x => x.ToTable("Section_GalleryFlickr"))
            //    .Map<LinkList>(x => x.ToTable("Section_LinkList")); 

            // tph associations
            //this.Map<HtmlContent>(x => x.Requires("SectionType").HasValue("HC"))
            //  .Map<ContactForm>(x => x.Requires("SectionType").HasValue("CF"));
        }
    }

}

