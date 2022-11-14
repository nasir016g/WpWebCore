using Microsoft.AspNetCore.Http;
using Nsr.Common.Core.Localization.Models;
using Nsr.RestClient;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wp.Core.Domain.Sections;
using Wp.Services.WebPages;

namespace Wp.Web.Mvc.Models.Sections
{
    //[Validator(typeof(ContactFormValidator))]
    public class ContactFormSectionModel : ILocalizedModel<ContactFormLocalizedModel>
    {
        public ContactFormSectionModel()
        {
            Locales = new List<ContactFormLocalizedModel>();
        }

        public int Id { get; set; }
        public int WebPageId { get; set; }
       
        [WpResourceDisplayName("Section.ContactForm.EmailTo")]
        public string EmailTo { get; set; }

        [WpResourceDisplayName("Section.ContactForm.EmailCc")]
        public string EmailCc { get; set; }

        [WpResourceDisplayName("Section.ContactForm.EmailBcc")]
        public string EmailBcc { get; set; }

        [WpResourceDisplayName("Section.ContactForm.Subject")]
        public string Subject { get; set; }

         //[AllowHtml]
        [WpResourceDisplayName("Section.ContactForm.IntroText")]
        public string IntroText { get; set; }

         //[AllowHtml]
        [WpResourceDisplayName("Section.ContactForm.ThankYouText")]
        public string ThankYouText { get; set; }

         [WpResourceDisplayName("Common.Description")]
         public string Description { get; set; }

        [WpResourceDisplayName("Section.ContactForm.NameEnabled")]
        public bool NameEnabled { get; set; }

        [WpResourceDisplayName("Section.ContactForm.ExtendedEnabled")]
        public bool ExtendedEnabled { get; set; }//displays address and telephone and mobile 
        public IList<ContactFormLocalizedModel> Locales { get; set; }
    }

    public class ContactFormLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [WpResourceDisplayName("Section.ContactForm.Subject")]
        public string Subject { get; set; }

        //[AllowHtml]
        [WpResourceDisplayName("Section.ContactForm.IntroText")]
        public string IntroText { get; set; }

        //[AllowHtml]
        [WpResourceDisplayName("Section.ContactForm.ThankYouText")]
        public string ThankYouText { get; set; }
    }

    //[Validator(typeof(ContactFormReadOnlyValidator))]
    public class ContactFormSectionReadOnlyModel : BaseReadOnlyModel
    {
        public ContactFormSectionReadOnlyModel()
        {
        }

        public ContactFormSectionReadOnlyModel(Section section, IWebPageService webPageService, IHttpContextAccessor httpContextAccessor)
            : base(section, webPageService, httpContextAccessor) { }

        public string IntroText { get; set; }

        [DataType(DataType.EmailAddress)]
        [WpResourceDisplayName("Section.ContactForm.EmailFrom")]
        public string EmailFrom { get; set; }

        [WpResourceDisplayName("Section.ContactForm.Name")] 
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [WpResourceDisplayName("Section.ContactForm.Message")] 
        public string Message { get; set; }

        [WpResourceDisplayName("Section.ContactForm.Address")]
        public string Address { get; set; }

        [WpResourceDisplayName("Section.ContactForm.Postcode")]
        public string Postcode { get; set; }

        [WpResourceDisplayName("Section.ContactForm.City")] // plaats
        public string City { get; set; }

        [WpResourceDisplayName("Section.ContactForm.Telephone")]
        public string Telephone { get; set; }

        [WpResourceDisplayName("Section.ContactForm.Mobile")]
        public string Mobile { get; set; }

        public bool NameEnabled { get; set; }
        public bool ExtendedEnabled { get; set; }
    }

    public class ContactFormSectionMailerModel
    {
        [Display(Name = "Uw naam")]
        [Required(ErrorMessage = "Uw naam is verplicht.")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Uw email")]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}