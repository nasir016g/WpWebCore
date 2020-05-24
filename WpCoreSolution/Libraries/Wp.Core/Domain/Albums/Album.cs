using System.Collections.Generic;

namespace Wp.Core.Domain.Albums
{
    public class Album : EntityAuditable
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }

    private ICollection<AlbumPhoto> _albumPhoto;
    public virtual ICollection<AlbumPhoto> AlbumPhotos
    {
        get { return _albumPhoto ?? (_albumPhoto = new List<AlbumPhoto>()); }
        set { _albumPhoto = value; }
         
    }
  }
}
