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
    public class SkillItemMap : EntityBaseConfiguration<SkillItem>
    {
        public SkillItemMap()
        {
            this.ToTable("Resume_SkillItem");
            //this.HasKey(s => s.EntityId);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);   
        }
    }
}
