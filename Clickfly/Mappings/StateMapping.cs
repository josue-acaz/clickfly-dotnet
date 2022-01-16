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
            builder.Property(model => model.id).IsRequired().HasColumnType("varchar(40)");
            builder.Property(model => model.name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(model => model.prefix).IsRequired().HasColumnType("varchar(10)");
            builder.HasKey(model => model.id);
            builder.ToTable("states");

            builder.HasData(new State{
                id = "5bc5d3c3-75e4-4a07-8424-b873cdaf97eb",
                name = "Acre",
                prefix = "AC",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "5bf811ff-f3cb-4d36-96f0-da7151fb843c",
                name = "Alagoas",
                prefix = "AL",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "70c5e30a-9e3b-4219-9cb6-b74bc699d3a3",
                name = "Amapá",
                prefix = "AP",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "aa27b7ef-a373-4788-b5cb-84c4294b0ebb",
                name = "Amazonas",
                prefix = "AM",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "aa705c29-4d43-4aed-ae41-44fc1855aa7e",
                name = "Bahia",
                prefix = "BA",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "2cc0e771-ff94-4e6c-9197-9faa6978e751",
                name = "Ceará",
                prefix = "CE",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "10289889-ff3f-4a02-ac90-597d436c6435",
                name = "Distrito Federal",
                prefix = "DF",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "1cc1e4c1-96c0-4bdc-aeee-0541e283a231",
                name = "Espírito Santo",
                prefix = "ES",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "0dfb4d34-9394-4b62-8ceb-d9bd485cf539",
                name = "Goiás",
                prefix = "GO",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "5a38a9bd-c329-49e0-9516-6761f4a7d606",
                name = "Maranhão",
                prefix = "MA",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "ceb3a4c4-b747-412b-a144-5ef8a3ced03e",
                name = "Mato Grosso",
                prefix = "MT",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "9cfc017e-4cee-4e48-8208-a5e3779d70c9",
                name = "Mato Grosso do Sul",
                prefix = "MS",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "95d20824-33f9-476d-b6d8-c42c463a360e",
                name = "Minas Gerais",
                prefix = "MG",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "989a917f-ca06-4c0c-a44b-ce852d6eb8db",
                name = "Pará",
                prefix = "PA",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "f1f817e7-742c-492f-8cfd-e65b6aa51527",
                name = "Paraíba",
                prefix = "PB",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "46478f1d-66cb-4cc4-a2ba-ef183deb4388",
                name = "Paraná",
                prefix = "PR",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "b73847c2-ac29-4787-8a14-5673bc048dae",
                name = "Pernambuco",
                prefix = "PE",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "9f68091d-2dae-458d-8a1b-dbdb7d3b36c2",
                name = "Piauí",
                prefix = "PI",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "7c12da9f-3002-4735-8824-f6c68126cf6f",
                name = "Rio de Janeiro",
                prefix = "RJ",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "d04e4f39-d4f8-4cac-9fa7-682aad5e8bbc",
                name = "Rio Grande do Norte",
                prefix = "RN",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "2bc661cf-71ed-481b-87a4-ef0e530658ad",
                name = "Rio Grande do Sul",
                prefix = "RS",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "a027f62e-f662-450e-8688-bc89628492f5",
                name = "Rondônia",
                prefix = "RO",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "18ee0c3e-7e1d-481d-8135-591afa8bfea0",
                name = "Roraima",
                prefix = "RR",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "b21165a6-0a4a-496d-8371-bb050bce09d9",
                name = "Santa Catarina",
                prefix = "SC",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "d2cd4e43-d325-4d21-88a0-3542b38d0438",
                name = "São Paulo",
                prefix = "SP",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "ad03e50f-235b-48d3-901a-e8167ce71924",
                name = "Sergipe",
                prefix = "SE",
                excluded = false,
                created_at = DateTime.Now,
            });

            builder.HasData(new State{
                id = "94d630ce-66d4-49b2-a133-684de51be69c",
                name = "Tocantins",
                prefix = "TO",
                excluded = false,
                created_at = DateTime.Now,
            });
        }
    }
}
