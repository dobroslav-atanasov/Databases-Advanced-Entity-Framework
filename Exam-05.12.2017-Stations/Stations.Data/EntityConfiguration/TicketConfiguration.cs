namespace Stations.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(t => t.CustomerCard)
                .WithMany(cc => cc.BoughtTickets)
                .HasForeignKey(t => t.CustomerCardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Trip)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}