using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;

namespace clickfly.Mappings
{
    public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(model => model.label).IsRequired().HasColumnType("varchar(120)");
            builder.HasKey(model => model.id);
            builder.ToTable("user_roles");

            builder.HasData(new UserRole{
                id = "0b3214f8-0f71-469b-8203-d8ba7fc158f2",
                name = "employee",
                label = "Funcionário",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new UserRole{
                id = "a3bd4997-cb6a-4a1c-aa59-2b27035f7b49",
                name = "manager",
                label = "Gerente",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new UserRole{
                id = "1e8b789e-c5a7-41e2-a7c2-2776f0b8b616",
                name = "administrator",
                label = "Administrador",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new UserRole{
                id = "6b3b800c-f438-4ffd-8ec4-a34c1c63e87a",
                name = "general_administrator",
                label = "Administrador Geral",
                excluded = false,
                created_at = DateTime.Now,
            });
        }
    }
}
