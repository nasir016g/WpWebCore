using System;
using System.Collections.Generic;
using System.Linq;
using Wp.Core;
using Wp.Core.Domain.Seo;
using Wp.Data;

namespace Wp.Services.Seo
{
    public partial class UrlRecordService : EntityService<UrlRecord>, IUrlRecordService
    {
        private readonly IBaseRepository<UrlRecord> _urlRecordRepo;

        public UrlRecordService(UnitOfWork unitOfWork, IBaseRepository<UrlRecord> urlRecordRepository) : base(unitOfWork, urlRecordRepository)
        {
            this._urlRecordRepo = urlRecordRepository;
        }

        #region Methods
        public virtual IList<UrlRecord> GetAll(string slug)
        {
            var query = _urlRecordRepo.Table;
            if (!String.IsNullOrWhiteSpace(slug))
                query = query.Where(ur => ur.Slug.Contains(slug));
            query = query.OrderBy(ur => ur.Slug);
            
            return query.ToList();
        }

        //public virtual UrlRecord GetById(int id)
        //{
        //    if (id == 0)
        //        return null;

        //    return _urlRecordRepo.GetById(id);
        //}

        public virtual UrlRecord GetBySlug(string slug)
        {
            if (String.IsNullOrEmpty(slug))
                return null;

            var query = from ur in _urlRecordRepo.Table
                        where ur.Slug == slug
                        select ur;
            var urlRecord = query.FirstOrDefault();
            return urlRecord;
        }               

        public virtual string GetActiveSlug(int entityId, string entityName, int languageId)
        {           
            var query = from ur in _urlRecordRepo.Table
                        where ur.EntityId == entityId &&
                        ur.EntityName == entityName &&
                        ur.LanguageId == languageId &&
                        ur.IsActive
                        orderby ur.EntityId descending
                        select ur.Slug;
            var slug = query.FirstOrDefault();
            //little hack here. nulls aren't cacheable so set it to ""
            if (slug == null)
                slug = "";
            return slug;          
        }

        public virtual void SaveSlug<T>(T entity, string slug, int languageId) where T : EntityAuditable, ISlugSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            int entityId = entity.Id;
            string entityName = typeof(T).Name;

            var query = from ur in _urlRecordRepo.Table
                        where ur.EntityId == entityId &&
                        ur.EntityName == entityName &&
                        ur.LanguageId == languageId
                        orderby ur.EntityId descending
                        select ur;
            var allUrlRecords = query.ToList();
            var activeUrlRecord = allUrlRecords.FirstOrDefault(x => x.IsActive);

            if (activeUrlRecord == null && !string.IsNullOrWhiteSpace(slug))
            {
                //find in non-active records with the specified slug
                var nonActiveRecordWithSpecifiedSlug = allUrlRecords
                    .FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !x.IsActive);
                if (nonActiveRecordWithSpecifiedSlug != null)
                {
                    //mark non-active record as active
                    nonActiveRecordWithSpecifiedSlug.IsActive = true;
                    Update(nonActiveRecordWithSpecifiedSlug);
                }
                else
                {
                    //new record
                    var urlRecord = new UrlRecord()
                    {
                        EntityId = entity.Id,
                        EntityName = entityName,
                        Slug = slug,
                        LanguageId = languageId,
                        IsActive = true,
                    };
                    Insert(urlRecord);
                }
            }

            if (activeUrlRecord != null && string.IsNullOrWhiteSpace(slug))
            {
                //disable the previous active URL record
                activeUrlRecord.IsActive = false;
                Update(activeUrlRecord);
            }

            if (activeUrlRecord != null && !string.IsNullOrWhiteSpace(slug))
            {
                //is it the same slug as in active URL record?
                if (activeUrlRecord.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                {
                    //yes. do nothing
                    //P.S. wrote this way for more source code readability
                }
                else
                {
                    //find in non-active records with the specified slug
                    var nonActiveRecordWithSpecifiedSlug = allUrlRecords
                        .FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !x.IsActive);
                    if (nonActiveRecordWithSpecifiedSlug != null)
                    {
                        //mark non-active record as active
                        nonActiveRecordWithSpecifiedSlug.IsActive = true;
                        Update(nonActiveRecordWithSpecifiedSlug);

                        //disable the previous active URL record
                        activeUrlRecord.IsActive = false;
                        Update(activeUrlRecord);
                    }
                    else
                    {
                        //insert new record
                        //we do not update the existing record because we should track all previously entered slugs
                        //to ensure that URLs will work fine
                        var urlRecord = new UrlRecord()
                        {
                            EntityId = entity.Id,
                            EntityName = entityName,
                            Slug = slug,
                            LanguageId = languageId,
                            IsActive = true,
                        };
                        Insert(urlRecord);

                        //disable the previous active URL record
                        activeUrlRecord.IsActive = false;
                        Update(activeUrlRecord);
                    }

                }
            }
        }

        #endregion
    }
}
