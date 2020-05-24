using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Localization;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Localization
{
    public class LocalizedPropertyMap : EntityBaseConfiguration<LocalizedProperty>
    {
        public override void Configure(EntityTypeBuilder<LocalizedProperty> entityBuilder) 
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("LocalizedProperty");


            entityBuilder.Property(lp => lp.LocaleKeyGroup).IsRequired().HasMaxLength(400);
            entityBuilder.Property(lp => lp.LocaleKey).IsRequired().HasMaxLength(400);
            entityBuilder.Property(lp => lp.LocaleValue).IsRequired();

            entityBuilder.HasOne(lp => lp.Language)
                .WithMany()
                .HasForeignKey(lp => lp.LanguageId);
        }
    }
}
