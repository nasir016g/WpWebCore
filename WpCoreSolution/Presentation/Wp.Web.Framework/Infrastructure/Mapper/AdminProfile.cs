using AutoMapper;
using Nsr.Common.Core;
using Wp.Core.Domain.Common;
using Wp.Core.Domain.Tenants;
using Wp.Core.Domain.WebPages;
using Wp.Web.Framework.Models.Admin;
using static Wp.Web.Framework.Models.Admin.WebPageModel;

namespace Wp.Web.Framework.Infrastructure
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<WebPage, WebPageModel>()
                .ReverseMap()
                //.ForMember(dest => dest.Roles, options => options.Ignore())
                .ForMember(dest => dest.Sections, options => options.Ignore())
                .ForMember(dest => dest.UpdatedOn, options => options.Ignore())
                .ForMember(dest => dest.CreatedOn, options => options.Ignore());

            CreateMap<WebPageRole, WebPageRoleModel>()
                .ReverseMap();

            CreateMap<Tenant, TenantModel>()
                .ReverseMap();


            #region Settings
            //CreateMap<LocalizationSettingsModel, LocalizationSettings>()
            //    .ForMember(dest => dest.DefaultAdminLanguageId, mo => mo.Ignore());
            //CreateMap<LocalizationSettings, LocalizationSettingsModel>();

            CreateMap<WebsiteSettingsModel, WebsiteSettings>()
                .ForMember(dest => dest.Theme, mo => mo.MapFrom(src => src.SelectedTheme));
            CreateMap<WebsiteSettings, WebsiteSettingsModel>()
                .ForMember(dest => dest.SelectedTheme, mo => mo.MapFrom(src => src.Theme))
                .ForMember(dest => dest.AvailableThemes, mo => mo.Ignore());
            #endregion

        }

    }
}
