using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nsr.Common.Core;
using Nsr.Common.Service.Configuration;
using System.Linq;
using Wp.Core;
using Wp.Core.Domain.Common;
using Wp.Services.Common;
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

    

        #region Localization

        //public ActionResult Localization()
        //{
        //    var entity = _settingService.LoadSetting<LocalizationSettings>();
        //    var model = entity.ToModel();
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Localization(LocalizationSettingsModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ErrorNotification("An error occured during updating localization settings.", false);
        //        return View(model);
        //    }

        //    var entity = model.ToEntity();
        //    _settingService.SaveSetting(entity);

        //    SuccessNotification("Localization settings updated successfully.", true);
        //    return RedirectToAction("Localization");
        //}

        #endregion       
    }
}
