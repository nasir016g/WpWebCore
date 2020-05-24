using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wp.Core.Domain.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.Localization;
using Wp.Web.Validators.Sections;

namespace Wp.Web.Models.Sections
{
    
    public class ContactFormSectionModel : ILocalizedModel<ContactFormLocalizedModel>
    {
        public ContactFormSectionModel()
        {
            Locales = new List<ContactFormLocalizedModel>();
        }

        public int Id { get; set; }
        public int WebPageId { get; set; }
       
        public string EmailTo { get; set; }

        public string EmailCc { get; set; }

        public string EmailBcc { get; set; }

        public string Subject { get; set; }

        public string IntroText { get; set; }

        public string ThankYouText { get; set; }

         public string Description { get; set; }

        public bool NameEnabled { get; set; }

        public bool ExtendedEnabled { get; set; }//displays address and telephone and mobile 
        public IList<ContactFormLocalizedModel> Locales { get; set; }
    }

    public class ContactFormLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        public string Subject { get; set; }

        public string IntroText { get; set; }

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

        public string EmailFrom { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string Telephone { get; set; }

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