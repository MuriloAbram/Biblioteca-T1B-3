using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using T1B_3Library.Domain.Entities;

namespace T1B_3Library.Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            // Nome da tabela
            builder.ToTable("Books");

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

            // Categoria/Gênero
            builder.Property(b => b.GenderId)
                .IsRequired();

            // Livro em destaque
            builder.Property(b => b.IsFeatured)
                .IsRequired();

            // Data de criação
            builder.Property(b => b.CreatedAt)
                .IsRequired();

            // Relacionamento Book -> Gender
            builder.HasOne(b => b.Gender)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}