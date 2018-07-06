namespace P03_SalesDatabase.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SalesDbContext : DbContext
    {
        public SalesDbContext()
        {
        }

        public SalesDbContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            if (!options.IsConfigured)
            {
                options.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProductConfiguration());

            builder.ApplyConfiguration(new CustomerConfiguration());

            builder.ApplyConfiguration(new StoreConfiguration());

            builder.ApplyConfiguration(new SaleConfiguration());
        }
    }
}