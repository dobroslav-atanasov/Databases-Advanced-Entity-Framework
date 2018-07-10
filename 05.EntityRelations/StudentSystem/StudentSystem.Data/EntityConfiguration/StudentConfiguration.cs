namespace P01_StudentSystem.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId);

            builder.Property(s => s.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100);

            builder.Property(s => s.PhoneNumber)
                .IsRequired(false)
                .IsUnicode(false)
                .HasDefaultValueSql("CHAR(10)");

            builder.Property(s => s.RegisteredOn)
                .IsRequired();

            builder.Property(s => s.Birthday)
                .IsRequired(false);
        }
    }
}