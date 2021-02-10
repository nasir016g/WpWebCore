using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Web.Mvc.Models.Sections;

namespace Wp.Web.Mvc.Models
{
    public class WebPageModel
    {
        public WebPageModel()
        {
            Sections = new List<BaseReadOnlyModel>();
        }

        public int Id { get; set; }
        public bool UserHasCreateRights { get; set; }
        public bool SidebarVisible { get; set; }
        public IList<BaseReadOnlyModel> Sections { get; set; }
        public IEnumerable<SelectListItem> AvailableSections { get; set; }
    }
}
