using System.Collections.Generic;
using Wp.Core.Domain.Common;
using Wp.Core.Domain.Localization;
using Wp.Core.Domain.Tenants;
using Wp.Core.Domain.WebPages;
using Wp.Web.Framework.Infrastructure.Mapper;
using Wp.Web.Framework.Models.Admin;

namespace Wp.Web.Framework.Extensions.Mapper
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Admin

        #region Admin-Settings

        //Website
        public static WebsiteSettingsModel ToModel(this WebsiteSettings entity)
        {
            return entity.MapTo<WebsiteSettings, WebsiteSettingsModel>();
        }

        public static WebsiteSettings ToEntity(this WebsiteSettingsModel model)
        {
            return model.MapTo<WebsiteSettingsModel, WebsiteSettings>();
        }

        public static WebsiteSettings ToEntity(this WebsiteSettingsModel model, WebsiteSettings destination)
        {
            return model.MapTo(destination);
        }

        //Localization
        public static LocalizationSettingsModel ToModel(this LocalizationSettings entity)
        {
            return entity.MapTo<LocalizationSettings, LocalizationSettingsModel>();
        }

        public static LocalizationSettings ToEntity(this LocalizationSettingsModel model)
        {
            return model.MapTo<LocalizationSettingsModel, LocalizationSettings>();
        }

        public static LocalizationSettings ToEntity(this LocalizationSettingsModel model, LocalizationSettings destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Admin - Webpage

        public static IEnumerable<WebPageModel> ToModels(this IEnumerable<WebPage> entities)
        {
            return entities.MapTo<IEnumerable<WebPage>, IEnumerable<WebPageModel>>();
        }

        public static WebPageModel ToModel(this WebPage entity)
        {
            return entity.MapTo<WebPage, WebPageModel>();
        }

        public static WebPage ToEntity(this WebPageModel model)
        {
            return model.MapTo<WebPageModel, WebPage>();
        }

        public static WebPage ToEntity(this WebPageModel model, WebPage destination)
        {
            return model.MapTo(destination);
        }

        #endregion

       

        #region Admin - Tenant

        public static IEnumerable<TenantModel> ToModels(this IEnumerable<Tenant> entities)
        {
            return entities.MapTo<IEnumerable<Tenant>, IEnumerable<TenantModel>>();
        }

        public static TenantModel ToModel(this Tenant entity)
        {
            return entity.MapTo<Tenant, TenantModel>();
        }

        public static Tenant ToEntity(this TenantModel model)
        {
            return model.MapTo<TenantModel, Tenant>();
        }

        public static Tenant ToEntity(this TenantModel model, Tenant destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #endregion
    }
}
