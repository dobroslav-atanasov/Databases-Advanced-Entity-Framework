namespace P03_FootballBetting.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .HasMaxLength(20);

            builder.Property(u => u.Email)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(u => u.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(u => u.Balance)
                .IsRequired();
        }
    }
}