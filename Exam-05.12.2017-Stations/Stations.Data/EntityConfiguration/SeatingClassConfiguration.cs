namespace Stations.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class SeatingClassConfiguration : IEntityTypeConfiguration<SeatingClass>
    {
        public void Configure(EntityTypeBuilder<SeatingClass> builder)
        {
            builder.HasAlternateKey(sc => sc.Name);

            builder.HasAlternateKey(sc => sc.Abbreviation);
        }
    }
}