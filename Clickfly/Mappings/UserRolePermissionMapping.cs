using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BCryptNet = BCrypt.Net.BCrypt;
using clickfly.Models;
using clickfly.Data;
using System.Collections.Generic;

namespace clickfly.Mappings
{
    public class UserRolePermissionMapping : IEntityTypeConfiguration<UserRolePermission>
    {
        public void Configure(EntityTypeBuilder<UserRolePermission> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model._create).IsRequired();
            builder.Property(model => model._read).IsRequired();
            builder.Property(model => model._update).IsRequired();
            builder.Property(model => model._delete).IsRequired();
            builder.Property(model => model.user_role_id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.permission_resource_id).IsRequired().HasColumnType("varchar(40)");
            builder.HasKey(model => model.id);
            builder.ToTable("user_role_permissions");

            // PERMISSION RESOURCES
            string aircrafts_id = "c7582447-6dff-46a0-ad56-c204b248a77c";
            string aircraft_images_id = "3a20e0e8-d235-4cb1-bcec-251e9fcbb405";
            string flights_id = "33b2af87-b10a-4bc9-8ef8-b7a82f308f25";
            string flight_segments_id = "f0eba241-dba4-42de-b9ca-379db1937d62";
            string air_taxi_bases_id = "7c6a5ebc-c741-4942-beb3-1446d1c9e464";
            string bookings_id = "9a4181d4-01e0-40be-856a-eb940cbe7a17";
            string campaigns_id = "30191b2f-2ab9-4fff-b369-ed8a114e001e";
            string push_notifications_id = "ba2029c0-b1ed-4811-b18a-e2d81c4d3378";
            string boardings_id = "f934de0c-424b-4726-afcc-9a0564372ab8";
            string double_checks_id = "2c4c1d8b-07bc-4e7b-bcc2-31704876f744";
            string contact_requests_id = "99e84be3-bbcb-4b35-adce-42e2e037322a";
            string users_id = "230565e6-571c-4620-b067-90205353528b";
            string air_taxis_id = "1002da1c-241c-42af-be77-e90791c5bbab";
            string aerodromes_id = "002ac8e8-90de-4c17-9684-5f9a8cb5fa83";
            string aircraft_models_id = "c572d66e-f212-4882-93f3-b9d1e932dc40";
            string system_logs_id = "359bcfde-a8a0-4f63-89d8-3e3070887a0b";
            string cities_id = "c030a948-2771-4064-898d-00fb675844e5";
            string states_id = "10332667-bf51-4da3-ade4-0a61451bca0f";
            string manufacturers_id = "30791f17-c95c-4dad-9efc-80053fe167a9";

            // -------- EMPLOYEE --------
            string employee_id = "0b3214f8-0f71-469b-8203-d8ba7fc158f2";

            // AERONAVES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = aircrafts_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // IMAGENS DE AERONAVES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = aircraft_images_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // VOOS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = flights_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // ETAPAS DO VOO
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = flight_segments_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // BASES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = air_taxi_bases_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // RESERVAS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = bookings_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // CAMPANHAS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = campaigns_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // NOTIFICAÇÕES PUSH
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = push_notifications_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // EMBARQUES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = employee_id,
                permission_resource_id = boardings_id,
                excluded = false,
                created_at = DateTime.Now,
            });
        
        
            // -------- MANGER --------
            string manager_id = "a3bd4997-cb6a-4a1c-aa59-2b27035f7b49";

            // AERONAVES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = aircrafts_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // IMAGENS DE AERONAVES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = aircraft_images_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // VOOS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = flights_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // ETAPAS DO VOO
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = flight_segments_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // BASES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = air_taxi_bases_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // RESERVAS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = bookings_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // DUPLA CHECAGEM
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = double_checks_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // CAMPANHAS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = campaigns_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // SOLICITAÇÕES DE CONTATO
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = contact_requests_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // NOTIFICAÇÕES PUSH
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = push_notifications_id,
                excluded = false,
                created_at = DateTime.Now,
            });
            
            // EMBARQUES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = manager_id,
                permission_resource_id = boardings_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // -------- ADMINISTRATOR --------
            string administrator_id = "1e8b789e-c5a7-41e2-a7c2-2776f0b8b616";

            // AERONAVES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = aircrafts_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // IMAGENS DE AERONAVES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = aircraft_images_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // VOOS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = flights_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // ETAPAS DO VOO
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = flight_segments_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // BASES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = air_taxi_bases_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // RESERVAS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = bookings_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // DUPLA CHECAGEM
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = double_checks_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // CAMPANHAS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = campaigns_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // SOLICITAÇÕES DE CONTATO
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = contact_requests_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // NOTIFICAÇÕES PUSH
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = push_notifications_id,
                excluded = false,
                created_at = DateTime.Now,
            });
            
            // EMBARQUES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = boardings_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // USUÁRIOS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = administrator_id,
                permission_resource_id = users_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // -------- GENERAL_ADMINISTRATOR --------
            string general_administrator_id = "6b3b800c-f438-4ffd-8ec4-a34c1c63e87a";

            // TÁXIS AÉREROS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = air_taxis_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // USUÁRIOS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = users_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // AERÓDROMOS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = aerodromes_id,
                excluded = false,
                created_at = DateTime.Now,
            });
            
            // MODELOS DE AERONAVES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = aircraft_models_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // LOGS DO SISTEMA
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = system_logs_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // CIDADES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = cities_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // ESTADOS
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = states_id,
                excluded = false,
                created_at = DateTime.Now,
            });

            // FABRICANTES
            builder.HasData(new UserRolePermission{
                id = Guid.NewGuid().ToString(),
                _create = true,
                _read = true,
                _update = true,
                _delete = true,
                user_role_id = general_administrator_id,
                permission_resource_id = manufacturers_id,
                excluded = false,
                created_at = DateTime.Now,
            });
        }
    }
}
