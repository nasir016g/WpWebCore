using FluentValidation;
using Nsr.Common.Core;
using Nsr.Common.Services;
using Nsr.RestClient.RestClients.Localization;
using Wp.Core;
using Wp.Web.Mvc.Models.Sections;

namespace Wp.Web.Mvc.Validators.Sections
{
    public class ContactFormValidator : AbstractValidator<ContactFormSectionModel>
    {
        private readonly IWorkContext _workContext;

        public ContactFormValidator(ILocalizationWebApi localizationWebApi, IWorkContext workContext)
        {
            var languageId = _workContext.Current.WorkingLanguageId;
            RuleFor(x => x.EmailTo).NotEmpty().WithMessage(localizationWebApi.GetResource("Section.ContactForm.EmailTo.Required", languageId).GetAwaiter().GetResult());
            RuleFor(x => x.EmailTo).EmailAddress().WithMessage(localizationWebApi.GetResource("Common.WrongEmail", languageId).GetAwaiter().GetResult());
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationWebApi.GetResource("Section.ContactForm.Subject.Required", languageId).GetAwaiter().GetResult());
            _workContext = workContext;
        }
    }

    public class ContactFormReadOnlyValidator : AbstractValidator<ContactFormSectionReadOnlyModel>
    {
        private readonly IWorkContext _workContext;

        public ContactFormReadOnlyValidator(ILocalizationWebApi localizationWebApi, IWorkContext workContext)
        {
            var languageId = _workContext.Current.WorkingLanguageId;
            RuleFor(x => x.Name).NotNull()
                .WithMessage(localizationWebApi.GetResource("Section.ContactForm.Name.Required", languageId).GetAwaiter().GetResult())
                .When(x => x.NameEnabled); 
            RuleFor(x => x.EmailFrom).NotEmpty().WithMessage(localizationWebApi.GetResource("Section.ContactForm.EmailFrom.Required", languageId).GetAwaiter().GetResult()); 
            RuleFor(x => x.EmailFrom).EmailAddress().WithMessage(localizationWebApi.GetResource("Common.WrongEmail", languageId).GetAwaiter().GetResult());
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationWebApi.GetResource("Section.ContactForm.Message.Required", languageId).GetAwaiter().GetResult());
            _workContext = workContext;

            //http://www.beabigrockstar.com/using-fluent-validation-with-asp-net-mvc-part-4-database-validation/
            //Custom(s =>
            //    {
            //        var model = (ContactForm)sectionService.GetById(s.Id);
            //        if(model.ShowName)                   
            //            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.Name.Required")).;

            //        return null;                 
            //    });

        }
    }
}