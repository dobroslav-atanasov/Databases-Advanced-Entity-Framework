namespace Stations.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TrainSeatConfiguration : IEntityTypeConfiguration<TrainSeat>
    {
        public void Configure(EntityTypeBuilder<TrainSeat> builder)
        {
            builder.HasOne(ts => ts.Train)
                .WithMany(t => t.TrainSeats)
                .HasForeignKey(ts => ts.TrainId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ts => ts.SeatingClass)
                .WithMany(sc => sc.TrainSeats)
                .HasForeignKey(ts => ts.SeatingClassId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}