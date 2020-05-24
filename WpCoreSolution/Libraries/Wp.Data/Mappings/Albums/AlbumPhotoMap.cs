using Wp.Core.Domain.Albums;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Albums
{
    public class AlbumPhotoMap : EntityBaseConfiguration<AlbumPhoto>
  {
    public AlbumPhotoMap()
    {
      this.ToTable("Album_Photo_Mapping");
      //this.HasKey(pp => pp.EntityId);

      //http://weblogs.asp.net/manavi/archive/2011/05/01/associations-in-ef-4-1-code-first-part-5-one-to-one-foreign-key-associations.aspx
      this.HasRequired(pp => pp.Photo)
        .WithMany()
        .HasForeignKey(pp => pp.PhotoId);
        //.WillCascadeOnDelete(false);       
    }
  }
}
