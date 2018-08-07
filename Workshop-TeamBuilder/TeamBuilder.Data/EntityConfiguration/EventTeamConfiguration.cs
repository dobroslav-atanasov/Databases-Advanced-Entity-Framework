namespace TeamBuilder.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EventTeamConfiguration : IEntityTypeConfiguration<EventTeam>
    {
        public void Configure(EntityTypeBuilder<EventTeam> builder)
        {
            builder.HasKey(et => new { et.EventId, et.TeamId });

            builder.HasOne(et => et.Event)
                .WithMany(e => e.EventTeams)
                .HasForeignKey(et => et.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(et => et.Team)
                .WithMany(t => t.EventTeams)
                .HasForeignKey(et => et.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}