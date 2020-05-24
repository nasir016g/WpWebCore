using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Wp.Core;
using Wp.Core.Caching;
using Wp.Core.Domain.Sections;
using Wp.Core.Domain.Security;
using Wp.Core.Domain.WebPages;
using Wp.Core.Interfaces.Repositories;

namespace Wp.Services.WebPages
{

    public class WebPageService : EntityService<WebPage>, IWebPageService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ICacheManager _cacheManager;
        private readonly IBaseRepository<WebPage> _webPageRepo;
        private readonly IBaseRepository<WebPageRole> _webPageRoleRepo;
        private readonly IBaseRepository<Section> _sectionRepo;

        public WebPageService(IHttpContextAccessor httpContext,
            ICacheManager cacheManager,
                              IUnitOfWork unitOfWork,
                              IBaseRepository<WebPage> webPageRepo,
                              IBaseRepository<WebPageRole> webPageRoleRepo,
                              IBaseRepository<Section> sectionRepo)
        : base(unitOfWork, webPageRepo)
        {
            _httpContext = httpContext;
            _cacheManager = cacheManager;
            _webPageRepo = webPageRepo;
            _webPageRoleRepo = webPageRoleRepo;
            _sectionRepo = sectionRepo;
        }

        public override IList<WebPage> GetAll()
        {
           return _cacheManager.Get("allpages", () =>
            {
                return base.GetAll();
            });
        }

        public IList<WebPage> GetPagesByParentId(int parentId)
        {
            return _webPageRepo.Table.Where(x => x.ParentId == parentId).OrderBy(x => x.DisplayOrder).ToList();
        }

        //public WebPage GetById(int id)
        //{
        //  var page = _webPageRepo.GetById(id);
        //  return page;      
        //}

        public WebPage GetBySectionId(int sectionId)
        {
            return _sectionRepo.GetById(sectionId).WebPage;
        }

        public WebPage GetByVirtualPath(string virtualPath)
        {
            if (virtualPath == "")
                return _webPageRepo.Table.FirstOrDefault();

            return _webPageRepo.Table.SingleOrDefault(x => x.VirtualPath.ToLower() == virtualPath.ToLower());
        }

        public override void Insert(WebPage webPage)
        {
            var p = GetByVirtualPath(webPage.VirtualPath);
            if (p == null)
            {
                _webPageRepo.Add(webPage);
                _unitOfWork.Complete();
            }
        }

        public IList<Section> GetSectionsByPageId(int webPageId)
        {
            return _sectionRepo.Table.Where(x => x.WebPageId == webPageId).ToList();
        }

        public IList<WebPageRole> GetRolesByPageId(int webPageId)
        {
            return _webPageRoleRepo.Table.AsNoTracking().Where(x => x.WebPageId == webPageId).ToList();
        }

        public void DeleteRolesByPageId(int webPageId)
        {
            var existingRoles = _webPageRoleRepo.Table.Where(x => x.WebPageId == webPageId);
            foreach (var r in existingRoles)
            {
                _webPageRoleRepo.Remove(r);
            }
            _unitOfWork.Complete();
        }

        public bool HasCreateRights(int webPageId)
        {
            if (IsAdminCurrentUser())
                return true;

            var webPage = _webPageRepo.GetById(webPageId);

            if (!webPage.Visible)
                return false;

            var roles = webPage.Roles.Where(x => x.PermissionLevel == PermissionLevel.Create).Select(x => x);
            foreach (var role in roles)
            {
                if (_httpContext.HttpContext.User.IsInRole(role.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasEditRights(int webPageId)
        {
            if (IsAdminCurrentUser())
                return true;

            var webPage = _webPageRepo.GetById(webPageId);

            if (!webPage.Visible)
                return false;

            var roles = webPage.Roles.Where(x => x.PermissionLevel == PermissionLevel.Edit || x.PermissionLevel == PermissionLevel.Create).Select(x => x);
            foreach (var role in roles)
            {
                if (_httpContext.HttpContext.User.IsInRole(role.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasViewRights(int webPageId)
        {
            if (IsAdminCurrentUser())
                return true;

            var webPage = _webPageRepo.GetById(webPageId);

            if (!webPage.Visible)
                return false;

            if (webPage.AllowAnonymousAccess || (!webPage.AllowAnonymousAccess && _httpContext.HttpContext.User.Identity.IsAuthenticated))
                return true;

            var roles = webPage.Roles.Where(x => x.PermissionLevel == PermissionLevel.Create || x.PermissionLevel == PermissionLevel.Edit || x.PermissionLevel == PermissionLevel.View).Select(x => x);
            foreach (var role in roles)
            {
                if (_httpContext.HttpContext.User.IsInRole(role.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAdminCurrentUser()
        {
            if (_httpContext.HttpContext.User.IsInRole(SystemRoleNames.Administrators))
            {
                return true;
            }

            return false;
        }
    }
}
