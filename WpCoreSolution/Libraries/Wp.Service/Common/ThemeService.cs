using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Web;

namespace Wp.Services.Common
{
    public class ThemeService : IThemeService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ThemeService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public string[] GetThemes()
        {
            //var themeFolder = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Themes"));
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Themes");
            var themeFolder = new DirectoryInfo(path);
            var themes = themeFolder.GetDirectories().Select(x => x.Name).OrderBy(x => x).ToArray();

            return themes;
        }
    }
}
