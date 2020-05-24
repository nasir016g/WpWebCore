using System.Collections.Generic;
using Wp.Core;
using Wp.Core.Domain.Sections;
using Wp.Core.Domain.WebPages;

namespace Wp.Services.WebPages
{
    public interface IWebPageService : IEntityService<WebPage>
    {
        //WebPage GetById(int id);
        IList<WebPage> GetPagesByParentId(int parentId);
        WebPage GetByVirtualPath(string virtualPath);
        WebPage GetBySectionId(int sectionId);

        IList<Section> GetSectionsByPageId(int webPageId);
        IList<WebPageRole> GetRolesByPageId(int webPageId);
        void DeleteRolesByPageId(int webPageId);

        // security permission for currently logged in user    
        bool HasCreateRights(int webPageId);
        bool HasEditRights(int webPageId);
        bool HasViewRights(int webPageId);
        bool IsAdminCurrentUser();
    }
}