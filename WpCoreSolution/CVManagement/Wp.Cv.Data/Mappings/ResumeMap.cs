﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wp.Cv.Core.Domain;

namespace Wp.Cv.Data.Mappings
{
    public class ResumeMap : EntityBaseConfiguration<Resume>
    {
        public override void Configure(EntityTypeBuilder<Resume> builder)
        {
            base.Configure(builder);
            builder.ToTable("Resume");
            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        }
      
    }
}
