using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Media;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Media
{
    public class PhotoMap : EntityBaseConfiguration<Photo>
    {
       public override void Configure(EntityTypeBuilder<Photo> entityBuilder) 
        {
            base.Configure(entityBuilder);
            entityBuilder.ToTable("Photo");
       }
    }
}
