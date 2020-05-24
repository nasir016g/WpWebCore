using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Core;

namespace Wp.Data.Mappings
{
    //public class EntityBaseConfiguration3<T> where T : Entity
    //{
    //    public EntityBaseConfiguration3(EntityTypeBuilder<T> entityBuilder)
    //    {
    //        entityBuilder.HasKey(e => e.Id);
    //    }
    //}

    public class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
