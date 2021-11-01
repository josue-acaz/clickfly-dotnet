using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clickfly.Models;

namespace clickfly.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(model => model.id).IsRequired().HasColumnType("UUID");
            builder.Property(model => model.name).IsRequired().HasColumnType("VARCHAR");
            builder.Property(model => model.email).IsRequired().HasColumnType("VARCHAR");
            builder.Property(model => model.username).IsRequired().HasColumnType("VARCHAR");
            builder.Property(model => model.role).IsRequired().HasColumnType("VARCHAR");
            builder.Property(model => model.password_hash).IsRequired().HasColumnType("VARCHAR");
        }
    }
}
