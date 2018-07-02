namespace P02_DatabaseFirst.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Configurations;

    public class SoftUniContext : DbContext
    {
        public SoftUniContext()
        {
        }

        public SoftUniContext(DbContextOptions<SoftUniContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<EmployeeProject> EmployeesProjects { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AddressConfiguration());

            builder.ApplyConfiguration(new DepartmentConfiguration());

            builder.ApplyConfiguration(new EmployeeConfiguration());

            builder.ApplyConfiguration(new EmployeeProjectConfiguration());

            builder.ApplyConfiguration(new ProjectConfiguration());

            builder.ApplyConfiguration(new TownConfiguration());
        }
    }
}