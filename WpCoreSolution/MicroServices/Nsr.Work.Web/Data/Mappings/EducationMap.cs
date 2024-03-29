﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Data.Mappings
{
    public class EducationMap : EntityBaseConfiguration<Education>
    {
        public override void Configure(EntityTypeBuilder<Education> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProfileEducation");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.HasOne(e => e.Resume)
                .WithMany(r => r.Educations)
                .HasForeignKey(e => e.ResumeId);
        }
        
    }
}
