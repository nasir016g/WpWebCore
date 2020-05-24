using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Seo;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Seo
{
    public partial class UrlRecordMap : EntityBaseConfiguration<UrlRecord>
    {
        public override void Configure(EntityTypeBuilder<UrlRecord> entityBuilder) 
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("UrlRecord");

            entityBuilder.Property(lp => lp.EntityName).IsRequired().HasMaxLength(400);
            entityBuilder.Property(lp => lp.Slug).IsRequired().HasMaxLength(400);
        }
    }
}
