using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;

namespace clickfly.Mappings
{
    public class TimezoneMapping : IEntityTypeConfiguration<Timezone>
    {
        public void Configure(EntityTypeBuilder<Timezone> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.gmt).IsRequired();
            builder.HasKey(model => model.id);
            builder.ToTable("timezones");

            builder.HasData(new Timezone{
                id = "627fd947-1062-4b1a-8c2c-7ef36cad279e",
                gmt = -2,
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new Timezone{
                id = "1235eb4b-9dd5-464f-9487-3c35a6e73a24",
                gmt = -3,
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new Timezone{
                id = "01bdd6a6-ef30-46ac-908f-74fd42bc9531",
                gmt = -4,
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new Timezone{
                id = "b2deaa96-4df2-404c-9ac1-3b58ac9d655b",
                gmt = -5,
                excluded = false,
                created_at = DateTime.Now,
            });
        }
    }
}
