namespace Stations.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TrainConfiguration : IEntityTypeConfiguration<Train>
    {
        public void Configure(EntityTypeBuilder<Train> builder)
        {
            builder.HasAlternateKey(t => t.TrainNumber);
        }
    }
}