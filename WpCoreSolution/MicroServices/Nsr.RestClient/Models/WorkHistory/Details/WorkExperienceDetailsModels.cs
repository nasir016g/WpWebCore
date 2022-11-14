using Nsr.Common.Core;

namespace Nsr.RestClient.Models.WorkHistories
{
    public class WorkExperienceDetailsModels : IEntity, ILocalizedEntity
    {
        public WorkExperienceDetailsModels()
        {
            Projects = new List<ProjectDetailModel>();
        }

        public int Id { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Name")]
        public string Name { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Period")]
        public string Period { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Function")]
        public string Function { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.City")]
        public string City { get; set; }

        [WpResourceDisplayName("Resume.Fields.Experiences.Tasks")]
        public string Tasks { get; set; }

        public int DisplayOrder { get; set; }

        public IList<ProjectDetailModel> Projects { get; set; }
    }

    public class ProjectDetailModel : IEntity, ILocalizedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technology { get; set; }
    }
}