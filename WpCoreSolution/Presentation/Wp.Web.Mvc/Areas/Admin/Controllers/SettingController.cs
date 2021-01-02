using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Wp.Core;
using Wp.Core.Domain.Common;
using Wp.Core.Domain.Localization;
using Wp.Services.Common;
using Wp.Services.Configuration;
using Wp.Web.Framework.Extensions.Mapper;
using Wp.Web.Framework.Models.Admin;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : BaseAdminController
    {
        private readonly ISettingService _settingService;
        private readonly IThemeService _themeService;
        private readonly IWorkContext _workContext;

        public SettingController(ISettingService settingService, IThemeService themeService, IWorkContext workContext)
        {
            _settingService = settingService;
            _themeService = themeService;
            _workContext = workContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Website

        public ActionResult Website()
        {
            var entity = _settingService.LoadSetting<WebsiteSettings>();

            var model = entity.ToModel();
            model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x });
            return View(model);
        }

        [HttpPost]
        public ActionResult Website(WebsiteSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                ErrorNotification("An error occured during updating website settings.", false);
                model.AvailableThemes = _themeService.GetThemes().Select(x => new SelectListItem { Text = x, Selected = model.SelectedTheme == x });
                return View(model);
            }

            var entity = model.ToEntity();
            _settingService.SaveSetting(entity);
            _workContext.ClearCurrentSession();

            SuccessNotification("Website settings updated successfully.", true);
            return RedirectToAction("Website");
        }

        #endregion

        #region Localization

        public ActionResult Localization()
        {
            var entity = _settingService.LoadSetting<LocalizationSettings>();
            var model = entity.ToModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Localization(LocalizationSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                ErrorNotification("An error occured during updating localization settings.", false);
                return View(model);
            }

            var entity = model.ToEntity();
            _settingService.SaveSetting(entity);

            SuccessNotification("Localization settings updated successfully.", true);
            return RedirectToAction("Localization");
        }

        #endregion       
    }
}
