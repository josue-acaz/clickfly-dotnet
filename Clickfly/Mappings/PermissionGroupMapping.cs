using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;

namespace clickfly.Mappings
{
    public class PermissionGroupMapping : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.user_id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.user_role_id).IsRequired().HasColumnType("varchar(40)");
            builder.HasKey(model => model.id);
            builder.ToTable("permission_groups");

            // CRIAR ADMINISTRADOR DO SISTEMA
            builder.HasData(new PermissionGroup{
                id = "dfa0022b-39ff-4b47-91be-6574a67b9ee0",
                user_id = "fa5533ef-249a-4a83-86f8-0a3d903adb5c",
                user_role_id = "6b3b800c-f438-4ffd-8ec4-a34c1c63e87a",
                allowed = true,
                excluded = false,
                created_at = DateTime.Now,
            });
        }
    }
}
