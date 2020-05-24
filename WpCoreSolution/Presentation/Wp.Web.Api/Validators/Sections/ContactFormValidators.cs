using FluentValidation;
using Wp.Services.Localization;
using Wp.Web.Models.Sections;

namespace Wp.Web.Validators.Sections
{
    public class ContactFormValidator : AbstractValidator<ContactFormSectionModel>
    {
        public ContactFormValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.EmailTo).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.EmailTo.Required"));
            RuleFor(x => x.EmailTo).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.Subject.Required"));           
        }
    }

    public class ContactFormReadOnlyValidator : AbstractValidator<ContactFormSectionReadOnlyModel>
    {
        public ContactFormReadOnlyValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotNull()
                .WithMessage(localizationService.GetResource("Section.ContactForm.Name.Required"))
                .When(x => x.NameEnabled); 
            RuleFor(x => x.EmailFrom).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.EmailFrom.Required")); 
            RuleFor(x => x.EmailFrom).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("Section.ContactForm.Message.Required")); 

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