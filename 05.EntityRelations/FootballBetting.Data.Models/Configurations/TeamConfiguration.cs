namespace P03_FootballBetting.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.TeamId);

            builder.Property(t => t.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(t => t.LogoUrl)
                .IsRequired();

            builder.Property(t => t.Initials)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(3)
                .HasColumnType("CHAR(3)");

            builder.Property(t => t.Budget)
                .IsRequired();

            builder.HasOne(t => t.PrimaryKitColor)
                .WithMany(pkc => pkc.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId);

            builder.HasOne(t => t.SecondaryKitColor)
                .WithMany(skc => skc.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId);

            builder.HasOne(t => t.Town)
                .WithMany(tw => tw.Teams)
                .HasForeignKey(t => t.TownId);
        }
    }
}