namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext()
        {
        }

        public HospitalDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(250);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(80);

                entity.Property(e => e.HasInsurance)
                    .HasDefaultValue(true);
            });

            builder.Entity<Visitation>(entity =>
            {
                entity.HasKey(e => e.VisitationId);

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("VisitationDate")
                    .HasColumnType("DATETIME2")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(250);

                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Visitations)
                    .HasForeignKey(e => e.PatientId)
                    .HasConstraintName("FK_Visitations_Patients");

                entity.Property(e => e.DoctorId)
                    .IsRequired(false);

                entity.HasOne(e => e.Doctor)
                    .WithMany(d => d.Visitations)
                    .HasForeignKey(e => e.DoctorId)
                    .HasConstraintName("FK_Visitations_Doctors");
            });

            builder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(e => e.DiagnoseId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(250);

                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(e => e.PatientId)
                    .HasConstraintName("FK_Diagnoses_Patients");
            });

            builder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.MedicamentId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(50);
            });

            builder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(e => new {e.PatientId, e.MedicamentId});

                entity.HasOne(e => e.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(e => e.PatientId)
                    .HasConstraintName("FK_PatientsMedicaments_Patients");

                entity.HasOne(e => e.Medicament)
                    .WithMany(m => m.Prescriptions)
                    .HasForeignKey(e => e.MedicamentId)
                    .HasConstraintName("FK_PatientsMedicaments_Medicaments");
            });

            builder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DoctorId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(100);

                entity.Property(e => e.Specialty)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(100);
            });
        }
    }
}