using System; // Importa tipos básicos como Guid

namespace T1B_3Library.Desktop.DTOs
{
    // DTO contendo a representação completa do Livro retornada/exibida pela API
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RealeaseYear { get; set; }
        /// <summary>
        /// Nome da categoria do jogo, retornado pela API para exibição no DataGridView
        /// </summary>
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    // DTO contendo apenas os dados necessários para cadastrar um novo livro
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RealeaseYear { get; set; }
        public bool IsFeatured { get; set; }
    }

    // DTO contendo os dados permitidos para atualizar um livro existente
    public class UpdateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RealeaseYear { get; set; }
        public bool IsFeatured { get; set; }
    }
}