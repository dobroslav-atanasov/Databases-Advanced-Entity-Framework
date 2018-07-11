namespace P03_FootballBetting.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.Username)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(u => u.Password)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(25);

            builder.Property(u => u.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(80);

            builder.Property(u => u.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(u => u.Balance)
                .IsRequired();
        }
    }
}