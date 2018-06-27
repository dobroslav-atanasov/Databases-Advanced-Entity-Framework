namespace P03_FootballBetting.Data.Models.Configurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
        {
            builder.HasKey(ps => new {ps.GameId, ps.PlayerId});

            builder.Property(ps => ps.Assists)
                .IsRequired();

            builder.Property(ps => ps.MinutesPlayed)
                .IsRequired();

            builder.Property(ps => ps.ScoredGoals)
                .IsRequired();

            builder.HasOne(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId);

            builder.HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId);
        }
    }
}
