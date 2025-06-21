using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Alpha.Domain;
using Alpha.Persistences.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Persistences.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Domain.FakeStoreProduct> fakeStoreProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new FakeStoreProductConfiguration());
        }

        
    }
}
