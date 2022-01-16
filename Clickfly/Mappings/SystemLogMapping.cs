using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;
using clickfly.Data;
using System.Collections.Generic;

namespace clickfly.Mappings
{
    public class SystemLogMapping : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.resource).IsRequired().HasColumnType("varchar(50)");
            builder.Property(model => model.resource_id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.action).IsRequired().HasColumnType("varchar(50)");
            builder.Property(model => model._object).IsRequired().HasColumnType("text");
            
            builder.HasKey(model => model.id);
            builder.ToTable("system_logs");
        }
    }
}
