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
    public class ExperienceMap : EntityBaseConfiguration<Experience>
    {
        public ExperienceMap()
        {
            this.ToTable("Resume_Experience");
            //this.HasKey(w => w.EntityId);
            this.Property(w => w.Name).IsRequired().HasMaxLength(200);
            this.HasRequired(w => w.Resume)
                .WithMany(r => r.Experiences)
                .HasForeignKey(s => s.ResumeId);
        }
    }
}
