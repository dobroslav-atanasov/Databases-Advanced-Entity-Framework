namespace BusTicketsSystem.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Birthday)
                .IsRequired(false);

            builder.Property(c => c.Gender)
                .IsRequired();

            builder.HasOne(c => c.HomeTown)
                .WithMany(t => t.Customers)
                .HasForeignKey(c => c.HomeTownId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}