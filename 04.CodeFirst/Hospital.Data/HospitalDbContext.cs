namespace P01_HospitalDatabase.Data
{
    using EntityConfiguration;
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

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PatientConfiguration());

            builder.ApplyConfiguration(new VisitationConfiguration());

            builder.ApplyConfiguration(new DiagnoseConfiguration());

            builder.ApplyConfiguration(new MedicamentConfiguration());

            builder.ApplyConfiguration(new PatientMedicamentConfiguration());

            builder.ApplyConfiguration(new DoctorConfiguration());
        }
    }
}