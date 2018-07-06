namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class DiagnoseConfiguration : IEntityTypeConfiguration<Diagnose>
    {
        public void Configure(EntityTypeBuilder<Diagnose> builder)
        {
            builder.HasKey(d => d.DiagnoseId);

            builder.Property(d => d.Name)
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(d => d.Comments)
                .IsUnicode()
                .HasMaxLength(250);

            builder.HasOne(d => d.Patient)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.PatientId);
        }
    }
}