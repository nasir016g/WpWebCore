using System.Collections.Generic;
using System.Linq;
using Wp.Core.Domain.Albums;
using Wp.Core.Domain.Media;
using Wp.Data;

namespace Wp.Services.Albums
{
    public class AlbumService : EntityService<Album>, IAlbumService
    {
        private readonly IEntityBaseRepository<Album> _albumRepo;
        private readonly IEntityBaseRepository<AlbumPhoto> _albumPhotoRepo;
        private readonly IEntityBaseRepository<Photo> _photoRepo;


        public AlbumService(IEntityBaseRepository<Album> albumRepo, IEntityBaseRepository<AlbumPhoto> albumPhotoRepo, IEntityBaseRepository<Photo> photoRepo)
        : base(albumRepo)
        {
            this._albumRepo = albumRepo;
            this._albumPhotoRepo = albumPhotoRepo;
            this._photoRepo = photoRepo;
        }
       
        public IList<Photo> GetPhotosByAlbum(Album album)
        {
            var list = new List<Photo>();
            foreach (var ap in album.AlbumPhotos.OrderBy(x => x.DisplayOrder))
            {
                list.Add(ap.Photo);
            }
            return list;
        }        

        #region Album

        public Album GetById(int id)
        {
            return _albumRepo.GetById(id);
        }

        public Album GetByName(string name)
        {
            return _albumRepo.Table.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public override void Delete(Album t)
        {
            if (t != null)
            {
                var a = _albumRepo.GetById(t.Id);
                _albumRepo.Delete(a);
                foreach (var photo in GetPhotosByAlbum(t))
                {
                    _photoRepo.Delete(photo);
                }
            }
        }
        #endregion

        #region AlbumPhoto
        public AlbumPhoto GetPhotoById(int id)
        {
            return _albumPhotoRepo.GetById(id);
        }

        public IList<AlbumPhoto> GetPhotosByAlbumId(int albumId)
        {
            return _albumPhotoRepo.Table.Where(x => x.AlbumId == albumId).OrderBy(x => x.DisplayOrder).ToList();
        }

        public void InsertPhoto(AlbumPhoto t)
        {
            _albumPhotoRepo.Save(t);
        }

        public void UpdatePhoto(AlbumPhoto newT)
        {
            AlbumPhoto old = _albumPhotoRepo.GetById(newT.Id);
            old.DisplayOrder = newT.DisplayOrder;
            old.Description = newT.Description;
            old.AltText = newT.AltText;
            old.OverlayText = newT.OverlayText;
            old.PhotoId = newT.PhotoId;

            // strange: saving old or newT, it doesn't matter??
            _albumPhotoRepo.Save(newT);
        }

        public void DeletePhoto(AlbumPhoto t)
        {
            AlbumPhoto ap = _albumPhotoRepo.GetById(t.Id);
            _albumPhotoRepo.Delete(ap);
        }
        #endregion

    }
}
