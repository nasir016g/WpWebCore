using AutoMapper;
using Wp.Core.Domain.Expenses;
using Wp.Core.Domain.Tenants;
using Wp.Core.Domain.WebPages;
using Wp.Web.Api.Models.Admin;

namespace Wp.Web.Api.Infrastructure
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<WebPage, WebPageModel>()
                .ReverseMap()
                .ForMember(dest => dest.Sections, options => options.Ignore())
                .ForMember(dest => dest.UpdatedOn, options => options.Ignore())
                .ForMember(dest => dest.CreatedOn, options => options.Ignore());


            CreateMap<Tenant, TenantModel>()
                .ReverseMap();

            CreateMap<Expense, ExpenseModel>()
                .ForMember(dest => dest.Date, mo => mo.MapFrom(src => src.Date.ToShortDateString()))
                .ReverseMap()
                .ForMember(dest => dest.ExpenseAccount, options => options.Ignore())
                .ForMember(dest => dest.ExpenseCategory, options => options.Ignore());
            CreateMap<ExpenseAccount, ExpenseAccountModel>().ReverseMap();
            CreateMap<ExpenseCategory, ExpenseCategoryModel>().ReverseMap();
            CreateMap<ExpenseTag, ExpenseTagModel>().ReverseMap();
               

        }

    }
}
