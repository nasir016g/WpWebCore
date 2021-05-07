using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wp.Core.Domain.Websites;

namespace Wp.Data.Mappings.Websites
{
    public class WebsiteMap : EntityBaseConfiguration<Website>
    {
        public override void Configure(EntityTypeBuilder<Website> builder)
        {
            base.Configure(builder);
            builder.ToTable("Website");
        }
    }
}
