namespace P03_FootballBetting.Data.Models.Configurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasKey(c => c.ColorId);

            builder.Property(c => c.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
        }
    }
}
