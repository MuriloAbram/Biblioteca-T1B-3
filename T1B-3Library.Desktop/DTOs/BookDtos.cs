using System; // Importa tipos básicos como Guid

namespace T1B_3Library.Desktop.DTOs
{
    // DTO contendo a representação completa do Livro retornada/exibida pela API
    public class BookDto
    {
        public Guid Id { get; set; }           // Identificador único (chave primária) do livro
        public string Title { get; set; } = string.Empty;       // Título do livro
        public string Author { get; set; } = string.Empty;      // Nome do autor do livro
        public string Status { get; set; } = string.Empty;       // Estado atual (ex: Disponível, Emprestado, Indisponível)
    }
    // DTO contendo apenas os dados necessários para cadastrar um novo livro
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;       // Título do livro a ser criado
        public string Author { get; set; } = string.Empty;      // Nome do autor
    }

    // DTO contendo os dados permitidos para atualizar um livro existente
    public class UpdateBookDto
    {
        public string Title { get; set; } = string.Empty;       // Novo título
        public string Author { get; set; } = string.Empty;      // Novo autor
        public string Status { get; set; } = string.Empty;       // Novo status do livro
    }
}