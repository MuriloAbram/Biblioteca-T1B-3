using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using T1B_3Library.Domain.Entities;

namespace T1B_3Library.Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        { 
            // Chave primária
            builder.HasKey(b => b.Id);

            // Título
            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Autor
            builder.Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(200);

            // Editora
            builder.Property(b => b.Publisher)
                .HasMaxLength(100);

            // Ano de publicação
            builder.Property(b => b.YearPublication)
                .IsRequired();

            // Livro em destaque
            builder.Property(b => b.IsFeatured)
                .IsRequired();

            // Data de criação
            builder.Property(b => b.CreatedAt)
                .IsRequired();
        }
    }
}