using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Common.Core.Localization.Models;

namespace Nsr.Common.Data.Mapping
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
