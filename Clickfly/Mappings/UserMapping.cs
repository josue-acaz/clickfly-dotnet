using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BCryptNet = BCrypt.Net.BCrypt;
using clickfly.Models;
using clickfly.Data;

namespace clickfly.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            string password_hash = BCryptNet.HashPassword("97531482");

            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(model => model.email).IsRequired().HasColumnType("varchar(120)");
            builder.Property(model => model.username).IsRequired().HasColumnType("varchar(50)");
            builder.Property(model => model.phone_number).HasColumnType("varchar(30)");
            builder.Property(model => model.emergency_phone_number).HasColumnType("varchar(30)");
            builder.Property(model => model.password_hash).IsRequired().HasColumnType("varchar(100)");
            builder.Property(model => model.air_taxi_id).HasColumnType("varchar(40)");
            builder.HasKey(model => model.id);
            builder.ToTable("users");

            // CRIAR ADMINISTRADOR DO SISTEMA
            builder.HasData(new User{
                id = "fa5533ef-249a-4a83-86f8-0a3d903adb5c",
                name = "JOSUE ACAZ DOS SANTOS DE OLIVEIRA",
                email = "josue.acaz@outlook.com",
                username = "jsoliveira",
                password_hash = password_hash,
                created_at = DateTime.Now,
                excluded = false,
            });
        }
    }
}
