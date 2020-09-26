//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Razor;
//using Microsoft.AspNetCore.Mvc.ViewEngines;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Linq;
//using Wp.Core;
//using Wp.Service.Helpers;

//namespace Wp.Web.Framework.ViewEngines.Razor
//{
//    /// <summary>
//    /// A view engine that is aware of Theme override capabilities.
//    /// </summary>
//    public class ThemeableViewEngine : IViewEngine
//    {
//        private readonly RazorViewEngine fallbackViewEngine = new RazorViewEngine();
//        private string lastTheme;
//        private RazorViewEngine lastEngine;
//        private readonly object @lock = new object();

//        private RazorViewEngine CreateRealViewEngine()
//        {
//            lock (@lock)
//            {
//                var workContext = ServiceLocator.Instance.GetService<IWorkContext>();
//                string theme = workContext.Current.WebSite.Theme;
//                try
//                {
//                    if (theme == lastTheme)
//                    {
//                        return lastEngine;
//                    }
//                }
//                catch (Exception)
//                {
//                    return fallbackViewEngine;
//                }

//                // Create a new razor view engine using the theme name when prioritizing names for resolving views
//                lastEngine = new RazorViewEngine();

//                lastEngine.PartialViewLocationFormats =
//                    new[]
//                    {
//                        "~/Themes/" + theme + "/Views/{1}/{0}.cshtml",
//                        "~/Themes/" + theme + "/Views/Shared/{0}.cshtml",
//                        "~/Themes/" + theme + "/Views/Shared/{1}/{0}.cshtml",
//                        "~/Themes/" + theme + "/Views/Extensions/{1}/{0}.cshtml",
//                        "~/Views/Extensions/{1}/{0}.cshtml",
//                        "~/Views/Sections/{1}/{0}.cshtml",
//                    }.Union(lastEngine.PartialViewLocationFormats).ToArray();

//                lastEngine.ViewLocationFormats =
//                    new[]
//                    {
//                        "~/Themes/" + theme + "/Views/{1}/{0}.cshtml",
//                        "~/Themes/" + theme + "/Views/Extensions/{1}/{0}.cshtml",
//                        "~/Views/Extensions/{1}/{0}.cshtml",
//                        "~/Views/Sections/{1}/{0}.cshtml",
//                    }.Union(lastEngine.ViewLocationFormats).ToArray();

//                lastEngine.MasterLocationFormats =
//                    new[]
//                    {
//                        "~/Themes/" + theme + "/Views/{1}/{0}.cshtml",
//                        "~/Themes/" + theme + "/Views/Extensions/{1}/{0}.cshtml",
//                        "~/Themes/" + theme + "/Views/Shared/{1}/{0}.cshtml",
//                        "~/Themes/" + theme + "/Views/Shared/{0}.cshtml",
//                        "~/Views/Extensions/{1}/{0}.cshtml",
//                    }.Union(lastEngine.MasterLocationFormats).ToArray();

//                lastTheme = theme;

//                return lastEngine;
//            }
//        }

//        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
//        {
//            return CreateRealViewEngine().FindPartialView(controllerContext, partialViewName, useCache);
//        }

//        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
//        {
//            return CreateRealViewEngine().FindView(controllerContext, viewName, masterName, useCache);
//        }

//        public void ReleaseView(ControllerContext controllerContext, IView view)
//        {
//            CreateRealViewEngine().ReleaseView(controllerContext, view);
//        }

//        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
//        {
//            throw new NotImplementedException();
//        }

//        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
