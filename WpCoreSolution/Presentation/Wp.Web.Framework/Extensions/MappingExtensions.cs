using System.Collections.Generic;
using Wp.Core.Domain.Expenses;
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

        #region Admin - Expense

        public static IEnumerable<ExpenseModel> ToModels(this IEnumerable<Expense> entities)
        {
            return entities.MapTo<IEnumerable<Expense>, IEnumerable<ExpenseModel>>();
        }

        public static ExpenseModel ToModel(this Expense entity)
        {
            return entity.MapTo<Expense, ExpenseModel>();
        }

        public static Expense ToEntity(this ExpenseModel model)
        {
            return model.MapTo<ExpenseModel, Expense>();
        }

        public static Expense ToEntity(this ExpenseModel model, Expense destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Admin - ExpenseAccount

        public static IEnumerable<ExpenseAccountModel> ToModels(this IEnumerable<ExpenseAccount> entities)
        {
            return entities.MapTo<IEnumerable<ExpenseAccount>, IEnumerable<ExpenseAccountModel>>();
        }

        public static ExpenseAccountModel ToModel(this ExpenseAccount entity)
        {
            return entity.MapTo<ExpenseAccount, ExpenseAccountModel>();
        }

        public static ExpenseAccount ToEntity(this ExpenseAccountModel model)
        {
            return model.MapTo<ExpenseAccountModel, ExpenseAccount>();
        }

        public static ExpenseAccount ToEntity(this ExpenseAccountModel model, ExpenseAccount destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Admin - ExpenseCategory

        public static IEnumerable<ExpenseCategoryModel> ToModels(this IEnumerable<ExpenseCategory> entities)
        {
            return entities.MapTo<IEnumerable<ExpenseCategory>, IEnumerable<ExpenseCategoryModel>>();
        }

        public static ExpenseCategoryModel ToModel(this ExpenseCategory entity)
        {
            return entity.MapTo<ExpenseCategory, ExpenseCategoryModel>();
        }

        public static ExpenseCategory ToEntity(this ExpenseCategoryModel model)
        {
            return model.MapTo<ExpenseCategoryModel, ExpenseCategory>();
        }

        public static ExpenseCategory ToEntity(this ExpenseCategoryModel model, ExpenseCategory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Admin - ExpenseTag

        public static IEnumerable<ExpenseTagModel> ToModels(this IEnumerable<ExpenseTag> entities)
        {
            return entities.MapTo<IEnumerable<ExpenseTag>, IEnumerable<ExpenseTagModel>>();
        }

        public static ExpenseTagModel ToModel(this ExpenseTag entity)
        {
            return entity.MapTo<ExpenseTag, ExpenseTagModel>();
        }

        public static ExpenseTag ToEntity(this ExpenseTagModel model)
        {
            return model.MapTo<ExpenseTagModel, ExpenseTag>();
        }

        public static ExpenseTag ToEntity(this ExpenseTagModel model, ExpenseTag destination)
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
