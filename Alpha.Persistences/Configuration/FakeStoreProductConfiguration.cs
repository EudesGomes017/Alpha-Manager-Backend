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
    internal class FakeStoreProductConfiguration : EntityConfigurationBase<FakeStoreProduct>
    {


        public override void ConfigureEntityFields(EntityTypeBuilder<FakeStoreProduct> builder)
        {
            builder.ToTable("FakeStoreProduct");

            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Category).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Image).IsRequired();
        }
    }
}
