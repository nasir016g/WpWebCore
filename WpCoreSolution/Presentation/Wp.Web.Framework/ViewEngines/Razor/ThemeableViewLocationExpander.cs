using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;

namespace Wp.Web.Framework.ViewEngines.Razor
{
    public class ThemeableViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string theme = context.Values["Theme"];

            viewLocations =
                    new[]
                    {
                        "/Themes/" + theme + "/Views/{1}/{0}.cshtml",
                        "/Themes/" + theme + "/Views/Shared/{0}.cshtml",
                        //"~/Themes/" + theme + "/Views/Shared/{1}/{0}.cshtml",
                        //"~/Themes/" + theme + "/Views/Extensions/{1}/{0}.cshtml",
                        //"~/Views/Extensions/{1}/{0}.cshtml",
                        //"~/Views/Sections/{1}/{0}.cshtml",
                    }.Union(viewLocations).ToArray();


            //replace the Views to MyViews..  
            //viewLocations = viewLocations.Select(s => s.Replace("Views", themeLocation));

            return viewLocations;
        }        

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var workContext = context.ActionContext.HttpContext.RequestServices.GetRequiredService<IWorkContext>();
            string theme = workContext.Current.WebSite.Theme;
            context.Values["Theme"] = theme;
        }
    }
}
