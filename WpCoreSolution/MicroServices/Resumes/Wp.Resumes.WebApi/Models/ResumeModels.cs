using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Wp.Localization.Core.Models;

namespace Wp.Resumes.WebApi.Models
{
    public class ResumeModel : ILocalizedModel<ResumeLocalizedModel>
    {
        public ResumeModel()
        {
            Locales = new List<ResumeLocalizedModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string LinkedIn { get; set; }
        public string Summary { get; set; }

        public string ApplicationUserName { get; set; }
        public IList<ResumeLocalizedModel> Locales { get; set; }
    }

    public class ResumeLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }
        public string Summary { get; set; }
    }
}