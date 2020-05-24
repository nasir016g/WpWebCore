using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Localization;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Localization
{
    class LanguageMap : EntityBaseConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> entityBuilder) 
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("Language");

            entityBuilder.Property(l => l.Name).IsRequired().HasMaxLength(100);
            entityBuilder.Property(l => l.LanguageCulture).IsRequired().HasMaxLength(20);
            entityBuilder.Property(l => l.UniqueSeoCode).HasMaxLength(2);
            entityBuilder.Property(l => l.FlagImageFileName).HasMaxLength(50);

        }
    }
}
