namespace P03_SalesDatabase.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100);

            builder.Property(c => c.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(80);
        }
    }
}