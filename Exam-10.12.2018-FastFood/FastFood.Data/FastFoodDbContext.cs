namespace FastFood.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FastFoodDbContext : DbContext
	{
		public FastFoodDbContext()
		{
		}

		public FastFoodDbContext(DbContextOptions options)
			: base(options)
		{
		}

	    public DbSet<Category> Categories { get; set; }

	    public DbSet<Employee> Employees { get; set; }

	    public DbSet<Item> Items { get; set; }

	    public DbSet<Order> Orders { get; set; }

	    public DbSet<OrderItem> OrderItems { get; set; }

	    public DbSet<Position> Positions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
		    builder.ApplyConfiguration(new CategoryConfiguration());

		    builder.ApplyConfiguration(new EmployeeConfiguration());

		    builder.ApplyConfiguration(new ItemConfiguration());

		    builder.ApplyConfiguration(new OrderConfiguration());

		    builder.ApplyConfiguration(new OrderItemConfiguration());

		    builder.ApplyConfiguration(new PositionConfiguration());
		}
	}
}