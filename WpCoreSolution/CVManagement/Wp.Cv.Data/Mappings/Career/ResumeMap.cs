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
    public class ResumeMap : EntityBaseConfiguration<Resume>
    {
        public ResumeMap()
        {
            this.ToTable("Resume");            
            this.Property(r => r.Name).IsRequired().HasMaxLength(100);             
                            
        }
    }
}
