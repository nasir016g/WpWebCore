using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Service.Navigation
{
    public class NavigationModel
    {
        public string Url { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string IconCssClass { get; set; }
        public bool HasChildren { get; set; }

        public List<NavigationModel> ChildLinks { get; set; }
    }
}
