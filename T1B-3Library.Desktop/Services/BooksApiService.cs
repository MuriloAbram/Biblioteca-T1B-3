using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using T1B_3Library.Desktop.DTOs;
using T1B_3Library.Desktop.Helpers;
using static System.Net.WebRequestMethods;

namespace T1B_3Library.Desktop.Services
{
    public class BooksApiService
    {
        private readonly HttpClientHelper _http;

        //Construtor - Inicializa junto com o código quando o mesmo é chamado.
        public BooksApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        ///<summary>
        /// Lista todas os livros via GET /api/books
        /// </summary>
        public async Task<List<BookResponseDto>> GetAllAsync()
        {
            try
            {
                var livros = await _http.GetAsync<List<BookResponseDto>>("/api/books");
                return livros ?? new List<BookResponseDto>();
            }
            catch
            {
                return new List<BookResponseDto>();
            }
        }

        /// <summary>
        /// Busca um livro específico por ID via GET /api/books/{id} 
        /// </summary>
        public async Task<BookResponseDto?> GetByIdAsync(int id)
        {
            return await _http.GetAsync<BookResponseDto>($"/api/books/{id}");
        }

        /// <summary>
        /// Cria um novo livro via POST /api/books.
        /// Requer perfil Admin (verificado pela API).
        /// </summary>
        /// <param name="dto">Dados do livro a ser criado</param>
        /// <returns>Livro criado ou null em caso de erro</returns>
        public async Task<(bool Success, BookResponseDto? Book, string ErrorMessage)>
            CreateAsync(CreateBookDto dto)
        {
            return await _http.PostAsync<BookResponseDto>("/api/books", dto);
        }

        /// <summary>
        /// Atualiza um livro existente via PUT /api/books/{id}.
        /// Requer perfil Admin (verificado pela API).
        /// </summary>
        public async Task<(bool Success, BookResponseDto? Book, string ErrorMessage)>
            UpdateAsync(int id, UpdateBookDto dto)
        {
            return await _http.PutAsync<BookResponseDto>($"/api/books/{id}", dto);
        }

        /// <summary>
        /// Exclui um livro via DELETE /api/books/{id}.
        /// Requer perfil Admin (verificado pela API).
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> DeleteAsync(int id)
        {
            return await _http.DeleteAsync($"/api/books/{id}");
        }
    }



}

