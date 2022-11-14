using Nsr.Common.Core;


namespace Nsr.RestClient.Models.WorkHistories
{
    public class WorkHistoryDetailsModel : IEntity, ILocalizedEntity
    {
        public WorkHistoryDetailsModel()
        {
            Educations = new List<EducationDetailsModels>();
            Skills = new List<SkillDetailsModels>();
            Experiences = new List<WorkExperienceDetailsModels>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }         
        public string Email { get; set; }                 
        public string Website { get; set; }
        public string LinkedIn { get; set; }   
       

        [WpResourceDisplayName("Common.DateOfBirth")]
        public string DateOfBirth { get; set; }

        [WpResourceDisplayName("Resume.Fields.Summary")]
        public string Summary { get; set; }

         [WpResourceDisplayName("Resume.Fields.Educations")]
        public IList<EducationDetailsModels> Educations { get; set; }

        [WpResourceDisplayName("Resume.Fields.Skills")]
        public IList<SkillDetailsModels> Skills { get; set; }

         [WpResourceDisplayName("Resume.Fields.Experiences")]
        public IList<WorkExperienceDetailsModels> Experiences { get; set; }
    }


}