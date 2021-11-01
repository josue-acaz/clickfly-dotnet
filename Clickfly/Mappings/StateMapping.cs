using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;

namespace clickfly.Mappings
{
    public class StateMapping : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("UUID");
            builder.Property(model => model.name).IsRequired().HasColumnType("VARCHAR");
            builder.Property(model => model.prefix).IsRequired().HasColumnType("VARCHAR");
        }
    }
}
