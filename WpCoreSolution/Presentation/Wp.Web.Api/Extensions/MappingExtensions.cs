using System.Security.Claims;
using Wp.Core.Domain.Sections;
using Wp.Core.Domain.Security;
using Wp.Core.Domain.WebPages;
using Wp.Services.Localization;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Models.Sections;
using Wp.Web.Api.Infrastructure.Mapper;
using Wp.Web.Api.Models;
using Wp.Web.Api.Models.Admin;
using System.Collections.Generic;
using Wp.Core.Domain.Tenants;
using Wp.Core.Domain.Expenses;

namespace Wp.Web.Api.Extensions.Mapper
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

        #region Sections

        #region HtmlContentSection

        public static HtmlContentSectionModel ToModel(this HtmlContentSection entity)
        {
            return entity.MapTo<HtmlContentSection, HtmlContentSectionModel>();
        }

        public static HtmlContentSection ToEntity(this HtmlContentSectionModel model)
        {
            return model.MapTo<HtmlContentSectionModel, HtmlContentSection>();
        }

        public static HtmlContentSection ToEntity(this HtmlContentSectionModel model, HtmlContentSection destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ContactFormSection

        public static ContactFormSectionModel ToModel(this ContactFormSection entity)
        {
            return entity.MapTo<ContactFormSection, ContactFormSectionModel>();
        }

        public static ContactFormSection ToEntity(this ContactFormSectionModel model)
        {
            return model.MapTo<ContactFormSectionModel, ContactFormSection>();
        }

        public static ContactFormSection ToEntity(this ContactFormSectionModel model, ContactFormSection destination)
        {
            return model.MapTo(destination);
        }

        #endregion


        #region ResumeSection

        public static ResumeSectionModel ToModel(this ResumeSection entity)
        {
            return entity.MapTo<ResumeSection, ResumeSectionModel>();
        }

        public static ResumeSection ToEntity(this ResumeSectionModel model)
        {
            return model.MapTo<ResumeSectionModel, ResumeSection>();
        }

        public static ResumeSection ToEntity(this ResumeSectionModel model, ResumeSection destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #endregion

        //webpage
        public static WebPageFrontEndModel ToFrontEndModel(this WebPage entity, IWebPageService webPageService, ISectionService sectionService, ClaimsPrincipal user, int languageId, ILocalizedEntityService leService)
        {
            if (entity == null)
                return null;

            bool userIsAdmin = user.IsInRole(SystemRoleNames.Administrators);
            bool userHasEditRights = webPageService.HasEditRights(entity.Id);
            bool userHasCreateRights = webPageService.HasCreateRights(entity.Id);

            var model = new WebPageFrontEndModel()
            {
                Id = entity.Id,
                VirtualPath = entity.VirtualPath,
                UserHasCreateRights = userHasCreateRights,
                AvailableSections = sectionService.GetAvailableSections()
            };

            foreach (var sectionEntity in entity.Sections)
            {
                var sm = GetSectionModel(sectionEntity, languageId);
                sm.Id = sectionEntity.Id;
                //sm.WebPage = sectionEntity.WebPage;
                sm.UserHasEditRights = userHasEditRights;
                sm.UserIsAdmin = userIsAdmin;
                model.Sections.Add(sm);
            }
            return model;
        }

        private static BaseReadOnlyModel GetSectionModel(Section entity, int languageId)
        {
            if (entity is HtmlContentSection)
            {
                var htmlContent = new HtmlContentSectionReadOnlyModel();
                htmlContent.Html = ((HtmlContentSection)entity).GetLocalized(x => x.Html, languageId);
                htmlContent.Controller = "HtmlContent";
                return htmlContent;
            }
            else if (entity is ContactFormSection)
            {
                var contactForm = new ContactFormSectionReadOnlyModel();
                contactForm.IntroText = ((ContactFormSection)entity).IntroText;
                contactForm.NameEnabled = ((ContactFormSection)entity).NameEnabled;
                contactForm.ExtendedEnabled = ((ContactFormSection)entity).ExtendedEnabled;
                contactForm.Controller = "ContactForm";
                return contactForm;
            }
            else if (entity is ResumeSection)
            {
                var resume = new ResumeSectionReadOnlyModel();
                resume.ApplicationUserName = ((ResumeSection)entity).ApplicationUserName;
                resume.Controller = "Resume";
                return resume;
            }
            return null;
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
