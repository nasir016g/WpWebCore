﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Data.Mappings
{
    public class ResumeMap : EntityBaseConfiguration<Resume>
    {
        public override void Configure(EntityTypeBuilder<Resume> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProfileResume");
            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        }
      
    }
}
