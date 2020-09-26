using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Core.Domain.Localization;
using Wp.Services.Seo;
using Wp.Web.Framework.Localization;

namespace Wp.Web.Framework.Seo
{
    /// <summary>
    /// Provides properties and methods for defining a SEO friendly route, and for getting information about the route.
    /// </summary>
    public class GenericPathRoute : LocalizedRoute
    {

        #region Fields

        private readonly IRouter _target;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="routeName">Route name</param>
        /// <param name="routeTemplate">Route template</param>
        /// <param name="defaults">Defaults</param>
        /// <param name="constraints">Constraints</param>
        /// <param name="dataTokens">Data tokens</param>
        /// <param name="inlineConstraintResolver">Inline constraint resolver</param>
        public GenericPathRoute(IRouter target, string routeName, string routeTemplate, RouteValueDictionary defaults,
            IDictionary<string, object> constraints, RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver)
            : base(target, routeName, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get route values for current route
        /// </summary>
        /// <param name="context">Route context</param>
        /// <returns>Route values</returns>
        protected RouteValueDictionary GetRouteValues(RouteContext context)
        {
            //remove language code from the path if it's localized URL
            var path = context.HttpContext.Request.Path.Value;
            if (SeoFriendlyUrlsForLanguagesEnabled && path.IsLocalizedUrl(context.HttpContext.Request.PathBase, false, out Language _))
                path = path.RemoveLanguageSeoCodeFromUrl(context.HttpContext.Request.PathBase, false);

            //parse route data
            var routeValues = new RouteValueDictionary(ParsedTemplate.Parameters
                .Where(parameter => parameter.DefaultValue != null)
                .ToDictionary(parameter => parameter.Name, parameter => parameter.DefaultValue));
            var matcher = new TemplateMatcher(ParsedTemplate, routeValues);
            matcher.TryMatch(path, routeValues);

            return routeValues;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Route request to the particular action
        /// </summary>
        /// <param name="context">A route context object</param>
        /// <returns>Task of the routing</returns>
        public override Task RouteAsync(RouteContext context)
        {

            //try to get slug from the route data
            var routeValues = GetRouteValues(context);
            if (!routeValues.TryGetValue("GenericSeName", out object slugValue) || string.IsNullOrEmpty(slugValue as string))
                return Task.CompletedTask;

            var slug = slugValue as string;

            //performance optimization, we load a cached verion here. It reduces number of SQL requests for each page load
            var urlRecordService = ServiceLocator.Instance.GetService<IUrlRecordService>();
           // var urlRecord = urlRecordService.GetBySlugCache(slug);
            //comment the line above and uncomment the line below in order to disable this performance "workaround"
            var urlRecord = urlRecordService.GetBySlug(slug);

            //no URL record found
            if (urlRecord == null)
                return Task.CompletedTask;

            //virtual directory path
            var pathBase = context.HttpContext.Request.PathBase;

            //if URL record is not active let's find the latest one
            if (!urlRecord.IsActive)
            {
                var activeSlug = urlRecordService.GetActiveSlug(urlRecord.EntityId, urlRecord.EntityName, urlRecord.LanguageId);
                if (string.IsNullOrEmpty(activeSlug))
                    return Task.CompletedTask;

                //redirect to active slug if found
                var redirectionRouteData = new RouteData(context.RouteData);
                //redirectionRouteData.Values[NopPathRouteDefaults.ControllerFieldKey] = "Common"; //"controller"
                //redirectionRouteData.Values[NopPathRouteDefaults.ActionFieldKey] = "InternalRedirect"; //"action"
                //redirectionRouteData.Values[NopPathRouteDefaults.UrlFieldKey] = $"{pathBase}/{activeSlug}{context.HttpContext.Request.QueryString}"; //"url"
                //redirectionRouteData.Values[NopPathRouteDefaults.PermanentRedirectFieldKey] = true; //"permanentRedirect"
                redirectionRouteData.Values["controller"] = "Common"; 
                redirectionRouteData.Values["action"] = "PageNotFound"; 
                redirectionRouteData.Values["url"] = $"{pathBase}/{activeSlug}{context.HttpContext.Request.QueryString}"; 
                redirectionRouteData.Values["permanentRedirect"] = true; 
               // context.HttpContext.Items["nop.RedirectFromGenericPathRoute"] = true;
                context.RouteData = redirectionRouteData;
                return _target.RouteAsync(context);
            }

            
            //since we are here, all is ok with the slug, so process URL
            var currentRouteData = new RouteData(context.RouteData);
            switch (urlRecord.EntityName.ToLowerInvariant())
            {
                case "webpage":
                    {
                        currentRouteData.Values["controller"] = "WebPage";
                        currentRouteData.Values["action"] = "DetailsView";
                        currentRouteData.Values["webpageid"] = urlRecord.EntityId;
                        currentRouteData.Values["SeName"] = urlRecord.Slug;
                    }
                    break;

                default:
                    {
                        //no record found
                        throw new Exception(string.Format("Not supported EntityName for UrlRecord: {0}", urlRecord.EntityName));
                    }                
            }
            context.RouteData = currentRouteData;

            //route request
            return _target.RouteAsync(context);
        }

        #endregion
    }
}

