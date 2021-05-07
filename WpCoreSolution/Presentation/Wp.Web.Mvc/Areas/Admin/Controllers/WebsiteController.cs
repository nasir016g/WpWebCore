using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Services.Common;
using Wp.Services.Websites;
using Wp.Web.Framework.Extensions.Mapper;
using Wp.Web.Framework.Models.Admin;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WebsiteController : BaseAdminController
    {
        private readonly IWebsiteService _websiteService;
        private readonly IThemeService _themeService;

        public WebsiteController(IWebsiteService websiteService, IThemeService themeService)
        {
            _websiteService = websiteService;
            _themeService = themeService;
        }
        

        // GET: WebsiteController/Edit/5
        public ActionResult Edit()
        {
            var entity = _websiteService.GetAll().First();
           
            var model = entity.ToModel();
            model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x });
            return View(model);
        }

        // POST: WebsiteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebsiteModel model)
        {
            var website = _websiteService.GetAll().First();
            if (!ModelState.IsValid)
            {
                model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x, Selected = model.SelectedTheme == x });
                return View(model);
            }
            website = model.ToEntity(website);

            _websiteService.Update(website);
            return RedirectToAction("Edit");

        }
      
    }
}
