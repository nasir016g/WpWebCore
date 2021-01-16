//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.AspNetCore.Routing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Wp.Core.Domain.Localization;
//using Wp.Services.Events;
//using Wp.Services.Localization;
//using Wp.Services.Seo;

//namespace Wp.Web.Mvc.Infrastructure.Routing
//{
//    public class SlugRouteTransformer : DynamicRouteValueTransformer
//    {
//        #region Fields

//        private readonly IEventPublisher _eventPublisher;
//        private readonly ILanguageService _languageService;
//        private readonly IUrlRecordService _urlRecordService;
//        private readonly LocalizationSettings _localizationSettings;

//        #endregion

//        #region Ctor

//        public SlugRouteTransformer(IEventPublisher eventPublisher,
//            ILanguageService languageService,
//            IUrlRecordService urlRecordService,
//            LocalizationSettings localizationSettings)
//        {
//            _eventPublisher = eventPublisher;
//            _languageService = languageService;
//            _urlRecordService = urlRecordService;
//            _localizationSettings = localizationSettings;
//        }

//        #endregion
//        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
//        {
//            if (values == null)
//                return new ValueTask<RouteValueDictionary>(values);

//            if (!values.TryGetValue("SeName", out var slugValue) || string.IsNullOrEmpty(slugValue as string))
//                return new ValueTask<RouteValueDictionary>(values);

//            var slug = slugValue as string;
//            var urlRecord = _urlRecordService.GetBySlug(slug);

//            //no URL record found
//            if (urlRecord == null)
//                return new ValueTask<RouteValueDictionary>(values);

//            //virtual directory path
//            var pathBase = httpContext.Request.PathBase;         
            

//            //since we are here, all is ok with the slug, so process URL
//            switch (urlRecord.EntityName.ToLowerInvariant())
//            {
//                case "webpage":
//                {
//                    values["controller"] = "WebPage";
//                    values["action"] = "DetailsView";
//                    values["webpageid"] = urlRecord.EntityId;
//                    values["SeName"] = urlRecord.Slug;
//                }
//                break;           
           
//            default:
//                {
//                    //no record found
//                    throw new Exception(string.Format("Not supported EntityName for UrlRecord: {0}", urlRecord.EntityName));
//                }
//            }

//            return new ValueTask<RouteValueDictionary>(values);
//        }
//    }
//}
