namespace PetClinic.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ProcedureAnimalAidConfiguration : IEntityTypeConfiguration<ProcedureAnimalAid>
    {
        public void Configure(EntityTypeBuilder<ProcedureAnimalAid> builder)
        {
            builder.HasKey(paa => new { paa.AnimalAidId, paa.ProcedureId });

            builder.HasOne(paa => paa.Procedure)
                .WithMany(p => p.ProcedureAnimalAids)
                .HasForeignKey(paa => paa.ProcedureId);

            builder.HasOne(paa => paa.AnimalAid)
                .WithMany(ai => ai.AnimalAidProcedures)
                .HasForeignKey(paa => paa.AnimalAidId);
        }
    }
}