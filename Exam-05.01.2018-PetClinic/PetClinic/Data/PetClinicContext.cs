namespace PetClinic.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext()
        {
        }

        public PetClinicContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }

        public DbSet<AnimalAid> AnimalAids { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Procedure> Procedures { get; set; }

        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }

        public DbSet<Vet> Vets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AnimalAidConfiguration());

            builder.ApplyConfiguration(new AnimalConfiguration());

            builder.ApplyConfiguration(new ProcedureAnimalAidConfiguration());

            builder.ApplyConfiguration(new ProcedureConfiguration());

            builder.ApplyConfiguration(new VetConfiguration());
        }
    }
}