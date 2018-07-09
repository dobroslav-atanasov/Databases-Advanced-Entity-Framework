namespace P03_SalesDatabase.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(250)
                .HasDefaultValue("No description");
        }
    }
}