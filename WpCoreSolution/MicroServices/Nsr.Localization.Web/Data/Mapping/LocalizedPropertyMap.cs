using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Common.Core.Localization;
using Nsr.Common.Data;
using Nsr.RestClient.Models.Localization;

namespace Nsr.Localization.Web.Data
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
