using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wp.Core.Domain.Security;
using Wp.Core.Domain.WebPages;
using Wp.Services.Sections;
using Wp.Services.Seo;
using Wp.Services.WebPages;
using Wp.Web.Framework.Extensions.Mapper;
using Wp.Web.Framework.Models.Admin;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WebPageController : BaseAdminController
    {

        private readonly IWebPageService _webPageService;
        private readonly ISectionService _sectionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public WebPageController(IWebPageService webPageService, ISectionService sectionService, IUrlRecordService urlRecordService, RoleManager<IdentityRole> roleManager)
        {
            _webPageService = webPageService;
            _sectionService = sectionService;
            _urlRecordService = urlRecordService;
            _roleManager = roleManager;

        }

        #region Utilities

        private List<WebPageModel.WebPageRoleModel> GetAllRoles(WebPage webPage)
        {
            List<WebPageModel.WebPageRoleModel> allModels = (from role in _roleManager.Roles
                                                             where !(role.Name == SystemRoleNames.Administrators.ToString() || role.Name == SystemRoleNames.Users.ToString())
                                                             select new WebPageModel.WebPageRoleModel { Name = role.Name }).ToList();

            foreach (var model in allModels)
            {
                var role = webPage.Roles.FirstOrDefault(x => x.Name == model.Name);
                if (role != null)
                {
                    model.PermissionLevel = role.PermissionLevel;
                }
            }

            return allModels;
        }

        private void PrepareWebPageModel(WebPage entity, WebPageModel model)
        {
            //model.AvailableParentWebPages = _webPageService.GetAll().Where(x => x.Id != page.Id).Select(x => new SelectListItem { Text = x.NavigationName, Value = x.Id.ToString(), Selected = page.ParentId == x.Id });
            //model.Roles = GetAllRoles(page);
            //model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x, Selected = page.Theme == x });

            //model.CustomAttributes = CustomAttributeHelper.PrepareCustomAttributes(page, _customAttributeService, _customAttributeParser);
            model.Roles = GetAllRoles(entity);
        }

        #endregion
        // GET: WebPageController
        public ActionResult Index()
        {
            var entities = _webPageService.GetAll();
            var models = entities.ToModels();

            return View(models);
        }        

        // GET: WebPageController/Create
        public ActionResult Create()
        {
            var model = new WebPageModel();
            //model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x });
            return View(model);
        }

        // POST: WebPageController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(WebPageModel model)
        {
            if (!ModelState.IsValid)
            {
                //model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x });
                return View(model);
            }

            var page = model.ToEntity();
            _webPageService.Insert(page);
            _urlRecordService.SaveSlug(page, page.VirtualPath, 0);
            return RedirectToAction(nameof(Index));
        }

        // GET: WebPageController/Edit/5
        public ActionResult Edit(int pageId)
        {
            WebPage page = _webPageService.GetById(pageId);
            var model = page.ToModel();

            PrepareWebPageModel(page, model);

            return View(nameof(Edit), model);
        }

        // POST: WebPageController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(WebPageModel model, IFormCollection form)
        {
            var page = _webPageService.GetById(model.Id);

            if (!ModelState.IsValid)
            {
                PrepareWebPageModel(page, model);
                //model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x });
                return View(model);
            }

            if (model.Roles != null)
            {
                foreach (var role in model.Roles)
                {
                    var found = page.Roles.FirstOrDefault(x => x.Name == role.Name);
                    if (found == null)
                    { // new
                        found = new WebPageRole();
                        found.Name = role.Name;
                        found.CreatedOn = DateTime.Now;
                        found.UpdatedOn = DateTime.Now;
                        page.Roles.Add(found);
                    }
                    found.PermissionLevel = role.PermissionLevel;
                }
            }

            page = model.ToEntity(page);

            //var webPageAttributes = ParseCustomWebPageAttributes(page, form);
            //_genericAttributeService.SaveAttribute(page, SystemWebPageAttributeNames.CustomWebPageAttributes, webPageAttributes);


            ////custom address attributes
            //var customAttributes = form.ParseCustomAddressAttributes(_customAttributeParser, _customAttributeService);
            //var customAttributeWarnings = _customAttributeParser.GetAttributeWarnings(customAttributes);
            //page.CustomAttributes = customAttributes;

            _webPageService.Update(page);

            _urlRecordService.SaveSlug(page, page.VirtualPath, 0);

            return RedirectToAction("Edit", new { pageId = page.Id });
        }

        // GET: WebPageController/Delete/5
        public ActionResult Delete(int id)
        {
            var page = _webPageService.GetById(id);
            var model = page.ToModel();
            return View(model);
        }

        // POST: WebPageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(WebPageModel model)
        {
            try
            {
                var page = _webPageService.GetById(model.Id);
                var sections = _webPageService.GetSectionsByPageId(model.Id);
                foreach (var section in sections)
                {
                    _sectionService.Delete(section);
                }

                _webPageService.Delete(page);

                var urlRecord = _urlRecordService.GetBySlug(page.VirtualPath);
                _urlRecordService.Delete(urlRecord);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
