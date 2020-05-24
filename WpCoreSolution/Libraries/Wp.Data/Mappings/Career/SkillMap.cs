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
    public class SkillMap : EntityBaseConfiguration<Skill>
    {
        public SkillMap()
        {
            this.ToTable("Resume_Skill");
            //this.HasKey(s => s.EntityId);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.HasRequired(s => s.Resume)
                .WithMany(r => r.Skills)
                .HasForeignKey(s => s.ResumeId);
            
        }
    }
}
