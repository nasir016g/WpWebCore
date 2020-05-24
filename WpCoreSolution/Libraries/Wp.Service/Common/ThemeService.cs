using System.IO;
using System.Linq;
using System.Web;

namespace Wp.Services.Common
{
    public class ThemeService : IThemeService
    {
        public string[] GetThemes()
        {
            var themeFolder = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Themes"));
            var themes = themeFolder.GetDirectories().Select(x => x.Name).OrderBy(x => x).ToArray();

            return themes;
        }
    }
}
