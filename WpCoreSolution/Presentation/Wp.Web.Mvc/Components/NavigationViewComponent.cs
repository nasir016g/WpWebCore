using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Service.Navigation;
using Wp.Services.WebPages;

namespace Wp.Web.Mvc.Components
{
    public class NavigationTopLevelViewComponent : ViewComponent
    {
        private readonly INavigationService _navigationService;

        public NavigationTopLevelViewComponent(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public IViewComponentResult Invoke()
        {
            var links = _navigationService.TopLevel();
            return View(links);
        }


        //[ChildActionOnly]
        //public ActionResult TopLevel()
        //{
        //    var links = GetLinksByParentId(0);
        //    return PartialView(links);
        //}

        //[ChildActionOnly]
        //public ActionResult SubLevel(int parentId)
        //{
        //    var links = GetLinksByParentId(parentId);
        //    return PartialView(links);
        //}


    }
}
