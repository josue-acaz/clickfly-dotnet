using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;
using clickfly.Data;

namespace clickfly.Mappings
{
    public class PermissionResourceMapping : IEntityTypeConfiguration<PermissionResource>
    {
        public void Configure(EntityTypeBuilder<PermissionResource> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(model => model._table).IsRequired().HasColumnType("varchar(50)");

            builder.HasKey(model => model.id);
            builder.ToTable("permission_resources");

            builder.HasData(new PermissionResource{
                id = "230565e6-571c-4620-b067-90205353528b",
                name = "Usuários",
                _table = "users",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "10048a9f-7da0-4213-af5f-6e1446a62b2a",
                name = "Clientes",
                _table = "customers",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "c7582447-6dff-46a0-ad56-c204b248a77c",
                name = "Aeronaves",
                _table = "aircrafts",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "002ac8e8-90de-4c17-9684-5f9a8cb5fa83",
                name = "Aeródromos",
                _table = "aerodromes",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "33b2af87-b10a-4bc9-8ef8-b7a82f308f25",
                name = "Voos",
                _table = "flights",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "f0eba241-dba4-42de-b9ca-379db1937d62",
                name = "Etapas do voo",
                _table = "flight_segments",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "30791f17-c95c-4dad-9efc-80053fe167a9",
                name = "Fabricantes",
                _table = "manufacturers",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "10332667-bf51-4da3-ade4-0a61451bca0f",
                name = "Estados",
                _table = "states",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "c030a948-2771-4064-898d-00fb675844e5",
                name = "Cidades",
                _table = "cities",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "1002da1c-241c-42af-be77-e90791c5bbab",
                name = "Táxis Aéreos",
                _table = "air_taxis",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "7c6a5ebc-c741-4942-beb3-1446d1c9e464",
                name = "Bases",
                _table = "air_taxi_bases",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "5f3beb7b-30b7-4b13-a6f9-2575e8ac80c3",
                name = "Newsletters",
                _table = "newsletters",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "f6c9d174-1a2c-49c3-be51-c9f24cbdd55a",
                name = "Assinantes",
                _table = "subscribers",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "3a20e0e8-d235-4cb1-bcec-251e9fcbb405",
                name = "Imagens da Aeronave",
                _table = "aircraft_images",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "9a4181d4-01e0-40be-856a-eb940cbe7a17",
                name = "Reservas",
                _table = "bookings",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "c572d66e-f212-4882-93f3-b9d1e932dc40",
                name = "Modelo de aeronaves",
                _table = "aircraft_models",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "faa82a99-ead4-454c-a9d3-7c62f4da7430",
                name = "Endereços do cliente",
                _table = "customer_addresses",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "b155a133-120c-4ded-9dfa-88316f4f790c",
                name = "Aeródromos do cliente",
                _table = "customer_aerodromes",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "b51a4bd6-5a70-44a7-85e7-2da9466c5d8c",
                name = "Passageiros",
                _table = "passengers",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "d1d313ca-d2a3-48ff-ac32-cfa842a633c2",
                name = "Configurações do Sistema",
                _table = "system_configs",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "359bcfde-a8a0-4f63-89d8-3e3070887a0b",
                name = "Logs do Sistema",
                _table = "system_logs",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "2237baef-2a10-471e-abe1-ee2994eafb77",
                name = "Contatos do cliente",
                _table = "customer_contacts",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "2c4c1d8b-07bc-4e7b-bcc2-31704876f744",
                name = "Duplas Checagens",
                _table = "double_checks",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "ba2029c0-b1ed-4811-b18a-e2d81c4d3378",
                name = "Notificações Push",
                _table = "push_notifications",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "f934de0c-424b-4726-afcc-9a0564372ab8",
                name = "Embarques",
                _table = "boardings",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "30191b2f-2ab9-4fff-b369-ed8a114e001e",
                name = "Campanhas",
                _table = "campaigns",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new PermissionResource{
                id = "99e84be3-bbcb-4b35-adce-42e2e037322a",
                name = "Solicitações de Contato (Site)",
                _table = "contact_requests",
                excluded = false,
                created_at = DateTime.Now,
            });

        }
    }
}
