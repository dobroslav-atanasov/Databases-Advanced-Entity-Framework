namespace BillsPaymentSystem.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasKey(cc => cc.CreditCardId);

            builder.Property(cc => cc.Limit)
                .IsRequired();

            builder.Property(cc => cc.MoneyOwed)
                .IsRequired();

            builder.Ignore(cc => cc.LimitLeft);

            builder.Property(cc => cc.ExpirationDate)
                .IsRequired();

            builder.Ignore(cc => cc.PaymentMethodId);
        }
    }
}