using System.Collections.Generic;
using System.Linq;
using Wp.Common;
using Wp.Core;
using Wp.Core.Domain.Media;
using Wp.Data;
using Wp.Services.Events;

namespace Wp.Services.Media
{
 

    public class PhotoService : EntityService<Photo>, IPhotoService
    {
        
        private readonly IBaseRepository<Photo> _photoRepository;

        public PhotoService(IUnitOfWork unitOfWork, IBaseRepository<Photo> photoRepo, IEventPublisher eventPublisher) : base(unitOfWork, photoRepo, eventPublisher)
        {
            _photoRepository = photoRepo;
        }

        #region Get methods
        //public Photo GetById(int id)
        //{
        //    return _photoRepository.GetById(id);
        //}
        
        #endregion`

        //public void Update(Photo newValues)
        //{
        //    Photo p = _photoRepository.GetById(newValues.Id);
        //    p.Name = newValues.Name;
        //    p.PhotoBinary = newValues.PhotoBinary;
        //    p.MimeType = newValues.MimeType;

        //    _photoRepository.Save(p);
        //}        
    }
}
