#pragma checksum "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "581cf7725b6adf2b93821f1527e349577db05cb0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_NavigationTopLevel_Default), @"mvc.1.0.view", @"/Views/Shared/Components/NavigationTopLevel/Default.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\_ViewImports.cshtml"
using Wp.Web.Mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\_ViewImports.cshtml"
using Wp.Web.Mvc.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
using Wp.Core;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"581cf7725b6adf2b93821f1527e349577db05cb0", @"/Views/Shared/Components/NavigationTopLevel/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ee4124c001d919046ef0298d0b02b132d6977151", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_NavigationTopLevel_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Wp.Service.Navigation.NavigationModel>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_LoginPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
  
    void DisplayIcon(string iconCssClass)
    {
        if (!String.IsNullOrWhiteSpace(iconCssClass))
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <i");
            BeginWriteAttribute("class", " class=\"", 212, "\"", 233, 1);
#nullable restore
#line 9 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
WriteAttributeValue("", 220, iconCssClass, 220, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></i>\r\n");
#nullable restore
#line 10 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
        }
        else
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
       Write(String.Empty);

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                         ;
        }
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"navbar-collapse collapse\">\r\n\r\n\r\n\r\n    <ul class=\"navbar-nav flex-grow-1\">\r\n");
#nullable restore
#line 23 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
         foreach (var link in Model)
        {
            if (link.HasChildren)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li class=\"nav-item dropdown\">\r\n                    <a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"navbarDropdown\" role=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">\r\n");
#nullable restore
#line 29 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                           DisplayIcon(@link.IconCssClass);

#line default
#line hidden
#nullable disable
            WriteLiteral("                        ");
#nullable restore
#line 30 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                   Write(link.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </a>\r\n                    <div class=\"dropdown-menu\" aria-labelledby=\"navbarDropdown\">\r\n");
#nullable restore
#line 33 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                         foreach (var subLink in link.ChildLinks)
                        {
                            //var path = applicationPath + subLink.Url;

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <a class=\"dropdown-item\"");
            BeginWriteAttribute("href", " href=\"", 1154, "\"", 1173, 1);
#nullable restore
#line 36 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
WriteAttributeValue("", 1161, subLink.Url, 1161, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n");
#nullable restore
#line 37 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                                   DisplayIcon(@link.IconCssClass);

#line default
#line hidden
#nullable disable
            WriteLiteral("                                ");
#nullable restore
#line 38 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                           Write(subLink.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </a>\r\n");
#nullable restore
#line 40 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </div>\r\n                </li>\r\n");
#nullable restore
#line 43 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
            }
            else
            {
                //var path = applicationPath + link.Url;

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li class=\"nav-item\">\r\n                    <a");
            BeginWriteAttribute("href", " href=\"", 1573, "\"", 1589, 1);
#nullable restore
#line 48 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
WriteAttributeValue("", 1580, link.Url, 1580, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"nav-link text-dark\">\r\n");
#nullable restore
#line 49 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                           DisplayIcon(@link.IconCssClass);

#line default
#line hidden
#nullable disable
            WriteLiteral("                        ");
#nullable restore
#line 50 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                   Write(link.Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </a>\r\n                </li>\r\n");
#nullable restore
#line 53 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
            }
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </ul>\r\n");
#nullable restore
#line 56 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
     if (Context.User.IsInRole(Wp.Core.Domain.Security.SystemRoleNames.Administrators.ToString()))
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <ul class=\"nav navbar-nav navbar-right\">\r\n            <li><a");
            BeginWriteAttribute("href", " href=\"", 1979, "\"", 2043, 1);
#nullable restore
#line 59 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
WriteAttributeValue("", 1986, Url.Action("Website", "Setting", new { area = "Admin" }), 1986, 57, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Admin</a></li>\r\n        </ul>\r\n");
#nullable restore
#line 61 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"col-md-2 pull-right\" style=\"top:5px;\"> ");
#nullable restore
#line 62 "C:\Git\WpWebCore\WpCoreSolution\Presentation\Wp.Web.Mvc\Views\Shared\Components\NavigationTopLevel\Default.cshtml"
                                                  Write(await Component.InvokeAsync("LanguageSelector"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "581cf7725b6adf2b93821f1527e349577db05cb011700", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Wp.Service.Navigation.NavigationModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
