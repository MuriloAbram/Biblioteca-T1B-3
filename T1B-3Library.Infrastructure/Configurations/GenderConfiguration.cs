using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using T1B_3Library.Domain.Entities;

namespace T1B_3Library.Infrastructure.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Genders");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Define relação 1:N com Book (reforço, BookConfiguration também contém a configuração)
            builder.HasMany(g => g.Books)
                .WithOne(b => b.Gender)
                .HasForeignKey(b => b.GenderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}