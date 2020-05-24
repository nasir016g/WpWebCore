using System.Collections.Generic;
using Wp.Core.Domain.Localization;

namespace Wp.Core.Domain.Career
{
    public class Resume : EntityAuditable, ILocalizedEntity
    {        
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string Website { get; set; }
        public string LinkedIn { get; set; }
        public string Summary { get; set; }
       
        public string ApplicationUserName { get; set; }
        // not working??
        //public virtual ApplicationUser ApplicationUser { get; set; }

        private ICollection<Experience> _experiences;
        public virtual ICollection<Experience> Experiences
        {
            get { return _experiences ?? (_experiences = new List<Experience>()); }
            set { _experiences = value; }
        }

        private ICollection<Education> _educations;
        public virtual ICollection<Education> Educations 
        {
            get {return _educations ?? (_educations = new List<Education>()); }
            set { _educations = value; }
        }

        private ICollection<Skill> _skills;
        public virtual ICollection<Skill> Skills
        {
            get { return _skills ?? (_skills = new List<Skill>()); }
            set { _skills = value; }
        }
    }
}
