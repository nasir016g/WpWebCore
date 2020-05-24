using System.Collections.Generic;
using Wp.Core;
using Wp.Core.Domain.Seo;

namespace Wp.Services.Seo
{
    public partial interface IUrlRecordService : IEntityService<UrlRecord>
    {
        IList<UrlRecord> GetAll(string slug);
        //UrlRecord GetById(int urlRecordId);
        UrlRecord GetBySlug(string slug);
        string GetActiveSlug(int entityId, string entityName, int languageId);
        void SaveSlug<T>(T entity, string slug, int languageId) where T : EntityAuditable, ISlugSupported;
    }

}