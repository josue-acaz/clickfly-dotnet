using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;

namespace clickfly.Mappings
{
    public class ManufacturerMapping : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.name).IsRequired().HasColumnType("varchar(50)");

            builder.HasKey(model => model.id);
            builder.ToTable("manufacturers");

            builder.HasData(new Manufacturer{
                id = "8fd21687-5beb-4f91-a13f-8dde04b6e24d",
                name = "Embraer",
                country = "BR",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new Manufacturer{
                id = "81bb3ca4-2a3f-40b0-b62a-6df2f0681e4b",
                name = "Piper Aircraft",
                country = "US",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new Manufacturer{
                id = "fdda563c-7109-45c3-bb26-f4f30c6975ad",
                name = "Cessna",
                country = "US",
                excluded = false,
                created_at = DateTime.Now,
            });
        }
    }
}
