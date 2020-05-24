using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core.Domain.Media;

namespace Wp.Core.Domain.Albums
{
    public class AlbumPhoto : EntityAuditable
  {

    public string Description { get; set; }
    public string OverlayText { get; set; }
    public string AltText { get; set; }
    public int DisplayOrder { get; set; }   

    public int PhotoId { get; set; }
    public virtual Photo Photo { get; set; }

    public int AlbumId { get; set; }
    public virtual Album Album { get; set; }
  }
}
