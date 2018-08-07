namespace TeamBuilder.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class TeamBuilderContext : DbContext
    {
        public TeamBuilderContext()
        {
        }

        public TeamBuilderContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<UserTeam> UserTeams { get; set; }

        public DbSet<EventTeam> EventTeams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new TeamConfiguration());

            builder.ApplyConfiguration(new EventConfiguration());

            builder.ApplyConfiguration(new InvitationConfiguration());

            builder.ApplyConfiguration(new UserTeamConfiguration());

            builder.ApplyConfiguration(new EventTeamConfiguration());
        }
    }
}