using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Frontend;

namespace Wp.Data.Mappings.Presentation
{
    public class FrontendWebPageMap : FrontendEntityBaseConfiguration<FrontendWebPage>
    {
        public override void Configure(EntityTypeBuilder<FrontendWebPage> entityBuilder)
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("WebPage");
        }

    }
}
