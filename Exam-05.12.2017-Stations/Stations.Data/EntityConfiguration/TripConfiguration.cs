namespace Stations.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasOne(t => t.Train)
                .WithMany(t => t.Trips)
                .HasForeignKey(t => t.TrainId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DestinationStation)
                .WithMany(s => s.TripsTo)
                .HasForeignKey(t => t.DestinationStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.OriginStation)
                .WithMany(s => s.TripsFrom)
                .HasForeignKey(t => t.OriginStationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}