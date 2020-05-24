using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Wp.Core.Domain.WebPages;
using Wp.Data.Mappings;

namespace Wp.Core.Mapping.WebPages
{
    public class WebPageMap : EntityBaseConfiguration<WebPage>
  {
        public override void Configure(EntityTypeBuilder<WebPage> entityBuilder) 
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("WebPage");
        }

    }
}
