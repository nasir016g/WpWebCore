using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core.Localization;
using Nsr.Common.Core.Localization.Models;
using Nsr.Common.Services;
using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Wp.Web.Framework
{
    //http://haacked.com/archive/2011/02/27/templated-razor-delegates.aspx/
    //http://www.prideparrot.com/blog/archive/2012/9/simplifying_html_generation_using_razor_templates
    public static class HtmlExtensions
    {
        public static IHtmlContent LocalizedEditor<T, TLocalizedModelLocal>(this IHtmlHelper<T> helper, string name,
            Func<int, HelperResult> localizedTemplate,
            Func<T, HelperResult> standardTemplate)
            where T : ILocalizedModel<TLocalizedModelLocal>
            where TLocalizedModelLocal : ILocalizedModelLocal
        {
            if (helper.ViewData.Model.Locales.Count > 1)
            {
                var tabStrip = new StringBuilder();
                tabStrip.Append(Tab<T, TLocalizedModelLocal>(helper).RenderHtmlContent());

                var tabContent = new TagBuilder("div");
                tabContent.AddCssClass("tab-content");

                var paneActive = new TagBuilder("div");
                paneActive.AddCssClass("tab-pane container active");
                paneActive.Attributes.Add("id", "standard");

                var test1 = standardTemplate(helper.ViewData.Model).RenderHtmlContent();

                paneActive.InnerHtml.AppendHtml(standardTemplate(helper.ViewData.Model).RenderHtmlContent());
                tabContent.InnerHtml.AppendHtml(paneActive.RenderHtmlContent());

                for (int i = 0; i < helper.ViewData.Model.Locales.Count; i++)
                {
                    var locale = helper.ViewData.Model.Locales[i];
                    var language = ServiceLocator.GetScope().ServiceProvider.GetService<ILanguageService>().GetById(locale.LanguageId);
                    var tabPane = new TagBuilder("div");
                    tabPane.AddCssClass("tab-pane");
                    tabPane.Attributes.Add("id", language.Name);
                    tabPane.InnerHtml.AppendHtml(localizedTemplate(i).RenderHtmlContent());
                    tabContent.InnerHtml.AppendHtml(tabPane.RenderHtmlContent());
                   
                }
                tabStrip.Append(tabContent.RenderHtmlContent());
                var str = tabStrip.ToString();
                return new HtmlString(tabStrip.ToString());
            }
            else
            {
               return new HtmlString(standardTemplate(helper.ViewData.Model).RenderHtmlContent());
            }            
        }

        private static TagBuilder Tab<T, TLocalizedModel>(this IHtmlHelper<T> helper)
           where T : ILocalizedModel<TLocalizedModel>
           where TLocalizedModel : ILocalizedModelLocal
        {
            var languageService = ServiceLocator.GetScope().ServiceProvider.GetService<ILanguageService>();
            var urlHelper = ServiceLocator.GetScope().ServiceProvider.GetService<IUrlHelperFactory>().GetUrlHelper(helper.ViewContext);

            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("nav nav-tabs");

            TagBuilder liStandard = new TagBuilder("li");
            liStandard.AddCssClass("nav-item");

            TagBuilder aStandard = new TagBuilder("a");
            aStandard.MergeAttribute("href", "#standard");
            aStandard.MergeAttribute("data-toggle", "tab");
            aStandard.AddCssClass("nav-link active");
            aStandard.Attributes.Add("value", "Standard");
            aStandard.InnerHtml.AppendHtml("Standard");

            liStandard.InnerHtml.AppendHtml(aStandard.RenderHtmlContent());
            ul.InnerHtml.AppendHtml(liStandard.RenderHtmlContent());

            foreach (var local in helper.ViewData.Model.Locales)
            {
                var language = languageService.GetById(local.LanguageId);
                TagBuilder li = new TagBuilder("li");
                li.AddCssClass("nav-item");

                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", string.Format("#{0}", language.Name));
                a.MergeAttribute("data-toggle", "tab");
                a.AddCssClass("nav-link");

                TagBuilder img = new TagBuilder("img");
                img.MergeAttribute("src", urlHelper.Content(string.Format("~/images/flags/{0}", language.FlagImageFileName)));
                img.MergeAttribute("alt", language.Name);
                img.Attributes.Add("style", "vertical-align:baseline");

                a.InnerHtml.AppendHtml(img.RenderHtmlContent());
                a.InnerHtml.AppendHtml(" " + language.Name);
                li.InnerHtml.AppendHtml(a.RenderHtmlContent());
                ul.InnerHtml.AppendHtml(li.RenderHtmlContent());
            }

            return ul;
        }

        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            var htmlOutput = writer.ToString();
            return htmlOutput;
        }

        public static string GetString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
