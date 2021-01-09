using Wp.Core.Domain.Albums;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Albums
{
    public class AlbumMap : EntityBaseConfiguration<Album>
    {
        public AlbumMap()
        {
            this.ToTable("Album");
            //this.HasKey(c => c.EntityId);
            this.Property(p => p.Name).IsRequired().HasMaxLength(50);
            this.Property(p => p.Description).IsMaxLength();
        }
    }
}
