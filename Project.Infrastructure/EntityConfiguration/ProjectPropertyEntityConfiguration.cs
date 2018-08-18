using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectPropertyEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<ProjectProperty> builder)
        {
            builder.ToTable("ProjectProperties")
                .Property(x => x.Key).IsRequired().HasMaxLength(100);

            builder.HasKey(p => new { p.ProjectId, p.Key, p.Value });
            builder.Property(x => x.Value).IsRequired().HasMaxLength(100);
        }
    }
}
