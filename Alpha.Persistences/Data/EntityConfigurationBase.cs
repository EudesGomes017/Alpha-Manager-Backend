using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Persistences.Data
{
    public abstract class EntityConfigurationBase<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.IsDeleted).IsRequired();


            ConfigureEntityFields(builder);

        }

        public abstract void ConfigureEntityFields(EntityTypeBuilder<T> builder);
    }
}
