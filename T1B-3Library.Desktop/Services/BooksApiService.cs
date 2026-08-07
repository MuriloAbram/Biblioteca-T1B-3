using System; // Importa tipos básicos como Guid
using System.Collections.Generic; // Importa coleções genéricas como IEnumerable e List
using System.Threading.Tasks; // Importa suporte a métodos assíncronos
using T1B_3Library.Desktop.DTOs; // Importa os DTOs de livros
using T1B_3Library.Desktop.Helpers; // Importa o HttpClientHelper

namespace T1B_3Library.Desktop.Services
{
    // Serviço responsável pelas operações de CRUD no endpoint de Livros da API
    public class BooksApiService
    {
        // Instância privada do cliente HTTP utilitário
        private readonly HttpClientHelper _httpHelper;

        // Construtor que recebe o cliente HTTP
        public BooksApiService(HttpClientHelper httpHelper)
        {
            _httpHelper = httpHelper; // Injeta a dependência
        }

        // Busca a lista completa de livros cadastrados
        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            // Executa requisição GET no endpoint "books" e retorna a lista (ou lista vazia em caso de nulo)
            return await _httpHelper.GetAsync<IEnumerable<BookDto>>("books") ?? new List<BookDto>();
        }

        // Busca os detalhes de um livro específico através do seu ID
        public async Task<BookDto?> GetByIdAsync(Guid id)
        {
            // Executa requisição GET especificando o ID da rota (ex: "books/1234-5678...")
            return await _httpHelper.GetAsync<BookDto>($"books/{id}");
        }

        // Cadastra um novo livro no sistema
        public async Task<BookDto?> CreateAsync(CreateBookDto createDto)
        {
            // Envia requisição POST com os dados do novo livro para o endpoint "books"
            return await _httpHelper.PostAsync<CreateBookDto, BookDto>("books", createDto);
        }

        // Atualiza os dados de um livro já existente
        public async Task<bool> UpdateAsync(Guid id, UpdateBookDto updateDto)
        {
            // Envia requisição PUT com o ID na URL e os dados atualizados no corpo da requisição
            return await _httpHelper.PutAsync($"books/{id}", updateDto);
        }

        // Remove um livro do acervo pelo ID
        public async Task<bool> DeleteAsync(Guid id)
        {
            // Envia requisição DELETE indicando o ID a ser removido
            return await _httpHelper.DeleteAsync($"books/{id}");
        }
    }
}