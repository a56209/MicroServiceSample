using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.Project>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.Project> builder)
        {
            builder.ToTable("Projects")
                .HasKey(p => p.Id);
        }
    }
}
