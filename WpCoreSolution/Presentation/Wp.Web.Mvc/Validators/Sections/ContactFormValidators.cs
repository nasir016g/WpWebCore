using FluentValidation;
using Nsr.Common.Core;
using Nsr.Common.Services;
using Wp.Core;
using Wp.Web.Mvc.Models.Sections;

namespace Wp.Web.Mvc.Validators.Sections
{
    public class ContactFormValidator : AbstractValidator<ContactFormSectionModel>
    {
        private readonly IWorkContext _workContext;

        public ContactFormValidator(ILocalizationService localizationService, IWorkContext workContext)
        {
            var languageId = _workContext.Current.WorkingLanguageId;
            RuleFor(x => x.EmailTo).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.EmailTo.Required", languageId));
            RuleFor(x => x.EmailTo).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail", languageId));
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.Subject.Required", languageId));
            _workContext = workContext;
        }
    }

    public class ContactFormReadOnlyValidator : AbstractValidator<ContactFormSectionReadOnlyModel>
    {
        private readonly IWorkContext _workContext;

        public ContactFormReadOnlyValidator(ILocalizationService localizationService, IWorkContext workContext)
        {
            var languageId = _workContext.Current.WorkingLanguageId;
            RuleFor(x => x.Name).NotNull()
                .WithMessage(localizationService.GetResource("Section.ContactForm.Name.Required", languageId))
                .When(x => x.NameEnabled); 
            RuleFor(x => x.EmailFrom).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.EmailFrom.Required", languageId)); 
            RuleFor(x => x.EmailFrom).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail", languageId));
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.Message.Required", languageId));
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