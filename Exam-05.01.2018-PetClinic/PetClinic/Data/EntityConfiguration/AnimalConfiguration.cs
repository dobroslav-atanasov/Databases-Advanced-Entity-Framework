namespace PetClinic.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.HasOne(a => a.Passport)
                .WithOne(p => p.Animal)
                .HasForeignKey<Animal>(a => a.PassportSerialNumber);
        }
    }
}