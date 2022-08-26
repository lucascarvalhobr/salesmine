﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesMine.Customers.API.Models;

namespace SalesMine.Customers.API.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.PublicPlace)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.Cep)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(c => c.Complement)
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Neighborhood)
                .HasColumnType("varchar(100)");

            builder.Property(c => c.City)
                .HasColumnType("varchar(100)");

            builder.Property(c => c.State)
                .HasColumnType("varchar(50)");

            builder.ToTable("TbAdress");
        }
    }
}
