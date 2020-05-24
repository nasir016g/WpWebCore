using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Localization;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Localization
{
    public class LocaleStringResourceMap : EntityBaseConfiguration<LocaleStringResource>
    {
        public override void Configure(EntityTypeBuilder<LocaleStringResource> entityBuilder) 
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("LocaleStringResource");
            entityBuilder.Property(lsr => lsr.ResourceName).IsRequired().HasMaxLength(200);
            entityBuilder.Property(lsr => lsr.ResourceValue).IsRequired();


            entityBuilder.HasOne(lsr => lsr.Language)
                .WithMany(l => l.LocaleStringResources)
                .HasForeignKey(lsr => lsr.LanguageId);
        }
    }
}
