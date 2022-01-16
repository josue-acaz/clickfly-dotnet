using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;
using clickfly.Data;
using System.Collections.Generic;

namespace clickfly.Mappings
{
    public class PermissionMapping : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model._create).IsRequired();
            builder.Property(model => model._read).IsRequired();
            builder.Property(model => model._update).IsRequired();
            builder.Property(model => model._delete).IsRequired();
            builder.HasKey(model => model.id);
            builder.ToTable("permissions");

            // CRIAR ADMINISTRADOR DO SISTEMA
            string users_id = "230565e6-571c-4620-b067-90205353528b";
            string air_taxis_id = "1002da1c-241c-42af-be77-e90791c5bbab";
            string aerodromes_id = "002ac8e8-90de-4c17-9684-5f9a8cb5fa83";
            string aircraft_models_id = "c572d66e-f212-4882-93f3-b9d1e932dc40";
            string system_logs_id = "359bcfde-a8a0-4f63-89d8-3e3070887a0b";
            string cities_id = "c030a948-2771-4064-898d-00fb675844e5";
            string states_id = "10332667-bf51-4da3-ade4-0a61451bca0f";
            string manufacturers_id = "30791f17-c95c-4dad-9efc-80053fe167a9";

            // GENERAL ADMINISTRATOR
            string permission_group_id = "dfa0022b-39ff-4b47-91be-6574a67b9ee0";

            // TÁXIS AÉREROS
            builder.HasData(new Permission{
                id = "29ce5517-9673-4595-94ec-3e17d13c8222",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = air_taxis_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // USUÁRIOS
            builder.HasData(new Permission{
                id = "b2a8ff46-9795-4e17-aeb0-29633bb3ffcf",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = users_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // AERÓDROMOS
            builder.HasData(new Permission{
                id = "e28bca5d-01d2-448a-98ca-c7aaf066b880",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = aerodromes_id,
                excluded = false,
                created_at = DateTime.Now,
            });
            
            // MODELOS DE AERONAVES
            builder.HasData(new Permission{
                id = "fb2f56aa-f2ff-4558-b9d9-571e3f64de89",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = aircraft_models_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // LOGS DO SISTEMA
            builder.HasData(new Permission{
                id = "4f408ec3-0e0e-43a9-adce-8866225994c5",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = system_logs_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // CIDADES
            builder.HasData(new Permission{
                id = "8637dad4-c8ec-44b1-b671-43c10fa43b0b",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = cities_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // ESTADOS
            builder.HasData(new Permission{
                id = "7013fd17-6bd9-4532-b33a-7a0800b2d182",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = states_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // FABRICANTES
            builder.HasData(new Permission{
                id = "8cdeb1ec-a823-40ff-a383-55366dabd9c7",
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                permission_group_id = permission_group_id,
                permission_resource_id = manufacturers_id,
                excluded = false,
                created_at = DateTime.Now,
            });
        }
    }
}
