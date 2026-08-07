using System; // Importa tipos básicos como Guid

namespace T1B_3Library.Desktop.DTOs
{
    // DTO contendo a representação completa do Livro retornada/exibida pela API
    public record BookDto(
        Guid Id,            // Identificador único (chave primária) do livro
        string Title,       // Título do livro
        string Author,      // Nome do autor do livro
        string Status       // Estado atual (ex: Disponível, Emprestado, Indisponível)
    );

    // DTO contendo apenas os dados necessários para cadastrar um novo livro
    public record CreateBookDto(
        string Title,       // Título do livro a ser criado
        string Author      // Nome do autor
    );

    // DTO contendo os dados permitidos para atualizar um livro existente
    public record UpdateBookDto(
        string Title,       // Novo título
        string Author,      // Novo autor
        string Status       // Novo status do livro
    );
}