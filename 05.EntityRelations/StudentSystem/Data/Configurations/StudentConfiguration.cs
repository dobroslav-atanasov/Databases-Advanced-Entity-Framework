namespace P01_StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(e => e.StudentId);

            builder.Property(e => e.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                .IsRequired(false)
                .IsUnicode(false)
                .HasColumnType("CHAR(10)");

            builder.Property(e => e.RegisteredOn)
                .IsRequired();

            builder.Property(e => e.Birthday)
                .IsRequired(false);
        }
    }
}