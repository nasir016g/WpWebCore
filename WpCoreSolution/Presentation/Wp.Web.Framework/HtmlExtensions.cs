using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.Common.Core.Localization.Models;
using Nsr.Common.Service.Localization;
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

        /// <summary>
        /// This method generates an HTML content for a localized model.
        /// It first checks if the model has more than one locale. If it does, it creates a tab structure with a tab for each locale and a "Standard" tab.
        /// Each tab is represented by a div element in a tab strip. The Tab helper method is used to create the tab strip.
        /// The localizedTemplate and standardTemplate functions are used to generate the content for each tab.
        /// The ServiceLocator.GetScope().ServiceProvider.GetService method is used to get services from the service provider.
        /// The IHtmlHelper.ViewData.Model.Locales property is used to get the locales of the model.
        /// The ILanguageService.GetById method is used to get a language by its ID.
        /// If the model has only one locale, it simply returns the standard template result.
        /// The StringBuilder, TagBuilder, and HtmlString classes are used to build the HTML content.
        /// The AddCssClass, Attributes.Add, and InnerHtml.AppendHtml methods are used to set the properties of the HTML elements.
        /// The RenderHtmlContent extension method is used to convert the HTML content of the elements to a string.
        /// The Func delegate is used to represent functions that take an integer or a model and return a HelperResult.
        /// The where keyword is used to specify constraints on the type parameters T and TLocalizedModelLocal.
        /// </summary>
        public static IHtmlContent LocalizedEditor<T, TLocalizedModelLocal>(this IHtmlHelper<T> helper, string name,
            Func<int, HelperResult> localizedTemplate,
            Func<T, HelperResult> standardTemplate)
            where T : ILocalizedModel<TLocalizedModelLocal>
            where TLocalizedModelLocal : ILocalizedModelLocal
        {
            // If the model has more than one locale...
            if (helper.ViewData.Model.Locales.Count > 1)
            {
                // Create a new StringBuilder for the tab strip.
                var tabStrip = new StringBuilder();
                tabStrip.Append(Tab<T, TLocalizedModelLocal>(helper).RenderHtmlContent());

                var tabContent = new TagBuilder("div");
                tabContent.AddCssClass("tab-content");

                var paneActive = new TagBuilder("div");
                paneActive.AddCssClass("tab-pane container active");
                paneActive.Attributes.Add("id", "standard");

                var test1 = standardTemplate(helper.ViewData.Model).RenderHtmlContent();

                // Append the standard template result to the active pane.
                paneActive.InnerHtml.AppendHtml(standardTemplate(helper.ViewData.Model).RenderHtmlContent());

                // Append the active pane to the tab content.
                tabContent.InnerHtml.AppendHtml(paneActive.RenderHtmlContent());


                // Loop through each locale in the model's Locales property.
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
            else  // If the model has only one locale...
            {
                // Return the standard template result as an HTML string.
                return new HtmlString(standardTemplate(helper.ViewData.Model).RenderHtmlContent());
            }
        }

        /// <summary>
        /// This method creates a tab structure for a localized model.
        /// It first creates a "Standard" tab and then creates a tab for each locale in the model's Locales property.
        /// Each tab is represented by a list item (li) element in an unordered list (ul) element.
        /// </summary>
        /// <typeparam name="T">The type of the model, which must implement the ILocalizedModel interface with TLocalizedModel as the type parameter.</typeparam>
        /// <typeparam name="TLocalizedModel">The type of the localized model, which must implement the ILocalizedModelLocal interface.</typeparam>
        /// <param name="helper">An instance of IHtmlHelper for the model.</param>
        /// <returns>A TagBuilder object representing the ul element with the tab structure.</returns>
        private static TagBuilder Tab<T, TLocalizedModel>(this IHtmlHelper<T> helper)
           where T : ILocalizedModel<TLocalizedModel>
           where TLocalizedModel : ILocalizedModelLocal
        {
            var languageService = ServiceLocator.GetScope().ServiceProvider.GetService<ILanguageService>();
            var urlHelper = ServiceLocator.GetScope().ServiceProvider.GetService<IUrlHelperFactory>().GetUrlHelper(helper.ViewContext);

            // Create a new unordered list (ul) element.
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("nav nav-tabs");

            // Create a new list item (li) for the "Standard" tab.
            TagBuilder liStandard = new TagBuilder("li");
            liStandard.AddCssClass("nav-item");

            // Create a new anchor (a) element for the "Standard" tab.
            TagBuilder aStandard = new TagBuilder("a");
            aStandard.MergeAttribute("href", "#standard");
            aStandard.MergeAttribute("data-toggle", "tab");
            aStandard.AddCssClass("nav-link active");
            aStandard.Attributes.Add("value", "Standard");
            aStandard.InnerHtml.AppendHtml("Standard");

            // Append the anchor element to the list item element.
            liStandard.InnerHtml.AppendHtml(aStandard.RenderHtmlContent());

            // Append the list item element to the unordered list element.
            ul.InnerHtml.AppendHtml(liStandard.RenderHtmlContent());

            // Loop through each locale in the model's Locales property.
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

        // Renders HTML content.
        // This is an extension method for the IHtmlContent interface that converts the HTML content into a string.
        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            var htmlOutput = writer.ToString();
            return htmlOutput;
        }

        // Retrieves string content from IHtmlContent.
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
