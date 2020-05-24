using System.Collections.Generic;
using Wp.Core.Domain.Albums;
using Wp.Core.Domain.Media;

namespace Wp.Services.Albums
{
    public interface IAlbumService : IEntityService<Album>
    {
        Album GetById(int id);
        Album GetByName(string name);

        AlbumPhoto GetPhotoById(int id);
        IList<AlbumPhoto> GetPhotosByAlbumId(int albumId);
        IList<Photo> GetPhotosByAlbum(Album album);
        void InsertPhoto(AlbumPhoto t);
        void UpdatePhoto(AlbumPhoto t);
        void DeletePhoto(AlbumPhoto t);
    }
}
