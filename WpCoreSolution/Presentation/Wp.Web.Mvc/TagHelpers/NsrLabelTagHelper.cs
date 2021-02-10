using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wp.Web.Mvc.TagHelpers
{
    [HtmlTargetElement("nsr-label", TagStructure = TagStructure.WithoutEndTag)]
    public class NsrLabelTagHelper : TagHelper
    {
        //<label class="col-sm-2 col-form-label" for="VirtualPath">VirtualPath</label>
               
        // PascalCase gets translated into kebab-case.
        public string For { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "label";    // Replaces <nsr-label> with <label> tag
            output.TagMode = TagMode.StartTagAndEndTag;
           
            output.Attributes.SetAttribute("for", For);
            var classValue = "control-label col-sm-3 text-sm-right font-weight-bold";
            output.Attributes.SetAttribute("class", classValue);
            output.Content.SetContent(For);
        }
    }
}
