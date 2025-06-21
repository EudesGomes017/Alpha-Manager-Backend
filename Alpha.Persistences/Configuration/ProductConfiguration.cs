using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Domain;
using Alpha.Persistences.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Persistences.Configuration
{
    internal class ProductConfiguration : EntityConfigurationBase<Product>
    {
        public override void ConfigureEntityFields(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.Property(p => p.Name).IsRequired().HasMaxLength(1000);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Barcode).IsRequired().HasMaxLength(50);
            builder.Property(p => p.ImageBytes).IsRequired(false);


        }
    }


    }