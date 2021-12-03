﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Wh.Core.Domain;

namespace Wp.Wh.Data.Mappings
{
    public class EducationMap : EntityBaseConfiguration<Education>
    {
        public override void Configure(EntityTypeBuilder<Education> builder)
        {
            base.Configure(builder);
            builder.ToTable("Education");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.HasOne(e => e.Resume)
                .WithMany(r => r.Educations)
                .HasForeignKey(e => e.ResumeId);
        }
        
    }
}