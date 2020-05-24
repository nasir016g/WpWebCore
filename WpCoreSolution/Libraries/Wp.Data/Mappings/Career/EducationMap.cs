using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wp.Core.Domain.Career;
using Wp.Data.Mappings;

namespace Wp.Core.Mappings.Career
{
    public class EducationMap : EntityBaseConfiguration<Education>
    {
        public EducationMap()
        {
            this.ToTable("Resume_Education");            
            this.Property(e => e.Name).IsRequired().HasMaxLength(200);
            this.HasRequired(e => e.Resume)
                .WithMany(r => r.Educations)
                .HasForeignKey(e => e.ResumeId);
            
        }
    }
}
