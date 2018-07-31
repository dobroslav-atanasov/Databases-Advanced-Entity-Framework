namespace ProductShop.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.Buyer)
                .WithMany(u => u.ProductBought)
                .HasForeignKey(p => p.BuyerId);

            builder.HasOne(p => p.Seller)
                .WithMany(u => u.ProductSold)
                .HasForeignKey(p => p.SellerId);
        }
    }
}