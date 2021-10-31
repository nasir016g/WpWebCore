﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Data.Mappings
{
    public class EducationItemMap : EntityBaseConfiguration<EducationItem>
    {
        public override void Configure(EntityTypeBuilder<EducationItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("EducationItem");
            builder.Property(ei => ei.Name).IsRequired().HasMaxLength(200);
            builder.HasOne(ei => ei.Education)
                .WithMany(e => e.EducationItems)
                .HasForeignKey(ei => ei.EducationId);
        }       
    }
}
