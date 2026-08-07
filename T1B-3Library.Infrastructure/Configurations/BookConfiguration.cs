using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using T1B_3Library.Domain.Entities;

namespace T1B_3Library.Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Description)
                .HasMaxLength(2000);

            builder.Property(b => b.Publisher)
                .HasMaxLength(100);

            // Relacionamento: Book -> Gender (opcional)
            builder.HasOne(b => b.Gender)
                .WithMany(g => g.Books)          // <--- usar Books (plural)
                .HasForeignKey(b => b.GenderId)  // <--- precisa existir em Book
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}