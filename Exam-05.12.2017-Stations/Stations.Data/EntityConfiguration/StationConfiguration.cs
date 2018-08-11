namespace Stations.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class StationConfiguration : IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.HasAlternateKey(s => s.Name);
        }
    }
}