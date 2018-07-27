namespace BusTicketsSystem.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => new { r.CustomerId, r.BusCompanyId });

            builder.Property(r => r.Content)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(r => r.Published)
                .IsRequired();

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.BusCompany)
                .WithMany(bs => bs.Reviews)
                .HasForeignKey(r => r.BusCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}