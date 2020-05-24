using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core.Domain.WebPages;
using Wp.Services.WebPages;

namespace Wp.Service.Navigation
{
    public class NavigationService
    {
        private readonly IWebPageService _webPageService;

        public NavigationService(IWebPageService webPageService)
        {
            _webPageService = webPageService;
        }

        #region Utilities

        private bool PageHasChildren(IEnumerable<WebPage> pages, int id)
        {
            return pages.Any(i => i.ParentId == id);
        }

        private IList<NavigationModel> GetLinksByParentId(int parentId)
        {
            var pages = _webPageService.GetPagesByParentId(parentId);
            var allPages = _webPageService.GetAll();
            List<NavigationModel> list = new List<NavigationModel>();
            foreach (var page in pages)
            {
                if (_webPageService.HasViewRights(page.Id))
                {
                    var item = new NavigationModel();

                    item.Text = page.NavigationName;
                    item.Value = page.Id.ToString();
                    item.Url = page.VirtualPath;
                   // item.IconCssClass = page.IconCssClass;
                    item.HasChildren = PageHasChildren(allPages, page.Id); //false; // default
                    item.ChildLinks = GetLinksByParentId(page.Id).ToList();
                    list.Add(item);
                }
            }
            return list;
        }


        #endregion

        public IList<NavigationModel> TopLevel()
        {
            var links = GetLinksByParentId(0);
            return links;
        }

        public IList<NavigationModel> SubLevel(int parentId)
        {
            var links = GetLinksByParentId(parentId);
            return links;
        }

    }
}
