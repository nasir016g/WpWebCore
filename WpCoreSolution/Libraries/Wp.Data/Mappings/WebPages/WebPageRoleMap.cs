using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.WebPages;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.WebPages
{
    public class WebPageRoleMap : EntityBaseConfiguration<WebPageRole>
  {
    public override void Configure(EntityTypeBuilder<WebPageRole> entityBuilder) 
        {
            base.Configure(entityBuilder);
            
            entityBuilder.ToTable("WebPage_Role_Mapping");
            entityBuilder.Ignore(r => r.PermissionLevel);
            entityBuilder.HasOne(r => r.WebPage)
            .WithMany(p => p.Roles)
            .HasForeignKey(r => r.WebPageId);
    }
  }
}
