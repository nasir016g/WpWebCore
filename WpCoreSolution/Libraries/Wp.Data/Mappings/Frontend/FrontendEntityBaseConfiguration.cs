using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core.Domain.Frontend;

namespace Wp.Data.Mappings.Presentation
{
    public class FrontendEntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : FrontendEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            //builder.HasKey(e => e.Id);
        }
    }
}
