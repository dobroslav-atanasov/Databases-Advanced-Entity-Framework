namespace Stations.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StationsDbContext : DbContext
    {
        public StationsDbContext()
        {
        }

        public StationsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Station> Stations { get; set; }

        public DbSet<Train> Trains { get; set; }

        public DbSet<SeatingClass> SeatingClasses { get; set; }

        public DbSet<TrainSeat> TrainSeats { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<CustomerCard> Cards { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StationConfiguration());

            builder.ApplyConfiguration(new TrainConfiguration());

            builder.ApplyConfiguration(new SeatingClassConfiguration());

            builder.ApplyConfiguration(new TrainSeatConfiguration());

            builder.ApplyConfiguration(new TripConfiguration());

            builder.ApplyConfiguration(new TicketConfiguration());
        }
    }
}