
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using T1B_3Library.Desktop.DTOs;
using T1B_3Library.Desktop.Helpers;

namespace T1B_3Library.Desktop.Services
{
    /// <summary>
    /// Serviço responsável pelas operações CRUD
    /// relacionadas aos livros da biblioteca.
    /// </summary>
    public class BooksApiService
    {
        // ================================================================
        // HTTP
        // ================================================================

        private readonly HttpClientHelper _httpHelper;


        // ================================================================
        // CONSTRUTOR
        // ================================================================

        public BooksApiService(
            HttpClientHelper httpHelper)
        {
            _httpHelper =
                httpHelper
                ?? throw new ArgumentNullException(
                    nameof(httpHelper)
                );
        }


        // ================================================================
        // GET - TODOS OS LIVROS
        // ================================================================

        /// <summary>
        /// Busca todos os livros cadastrados.
        /// </summary>
        public async Task<IEnumerable<BookDto>>
            GetAllAsync()
        {
            var books =
                await _httpHelper.GetAsync<
                    IEnumerable<BookDto>
                >("books");

            return books
                   ?? new List<BookDto>();
        }


        // ================================================================
        // GET - LIVRO POR ID
        // ================================================================

        /// <summary>
        /// Busca um livro pelo ID.
        /// </summary>
        public async Task<BookDto?>
            GetByIdAsync(Guid id)
        {
            return await _httpHelper.GetAsync<BookDto>(
                $"books/{id}"
            );
        }


        // ================================================================
        // POST - CRIAR LIVRO
        // ================================================================

        /// <summary>
        /// Cadastra um novo livro.
        /// </summary>
        public async Task<BookDto?>
            CreateAsync(
                CreateBookDto createDto)
        {
            if (createDto == null)
            {
                throw new ArgumentNullException(
                    nameof(createDto)
                );
            }

            return await _httpHelper.PostAsync<
                CreateBookDto,
                BookDto
            >(
                "books",
                createDto
            );
        }


        // ================================================================
        // PUT - ATUALIZAR LIVRO
        // ================================================================

        /// <summary>
        /// Atualiza os dados de um livro.
        /// </summary>
        public async Task<bool>
            UpdateAsync(
                Guid id,
                UpdateBookDto updateDto)
        {
            if (updateDto == null)
            {
                throw new ArgumentNullException(
                    nameof(updateDto)
                );
            }

            return await _httpHelper.PutAsync(
                $"books/{id}",
                updateDto
            );
        }


        // ================================================================
        // DELETE - EXCLUIR LIVRO
        // ================================================================

        /// <summary>
        /// Remove um livro pelo ID.
        /// </summary>
        public async Task<bool>
            DeleteAsync(Guid id)
        {
            var result =
                await _httpHelper.DeleteAsync(
                    $"books/{id}"
                );

            // Retorna somente o bool da tupla
            return result.Success;
        }
    }
}

