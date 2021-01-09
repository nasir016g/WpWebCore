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
    public class ProjectMap : EntityBaseConfiguration<Project>
    {
        public ProjectMap()
        {
            this.ToTable("Resume_Project");
            //this.HasKey(p => p.EntityId);
            this.Property(p => p.Name).IsRequired().HasMaxLength(200);
            this.HasRequired(p => p.Experience)
                .WithMany(w => w.Projects)
                .HasForeignKey(p => p.ExperienceId);
        }
    }
}
