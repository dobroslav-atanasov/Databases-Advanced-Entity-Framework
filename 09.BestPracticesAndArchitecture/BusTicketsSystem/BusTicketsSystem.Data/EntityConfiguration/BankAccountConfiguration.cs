namespace BusTicketsSystem.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(ba => ba.Id);

            builder.HasOne(ba => ba.Customer)
                .WithMany(c => c.BankAccounts)
                .HasForeignKey(ba => ba.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}