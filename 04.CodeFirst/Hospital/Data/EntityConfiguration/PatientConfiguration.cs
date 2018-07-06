namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.PatientId);

            builder.Property(p => p.FirstName)
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(p => p.LastName)
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(p => p.Address)
                .IsUnicode()
                .HasMaxLength(250);

            builder.Property(p => p.Email)
                .IsUnicode(false)
                .HasMaxLength(80);

            builder.Property(p => p.HasInsurance)
                .HasDefaultValue(true);
        }
    }
}