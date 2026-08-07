using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1B_3Library.Application.DTOs;
using T1B_3Library.Application.Interfaces;
using T1B_3Library.Domain.Entities;

namespace T1B_3Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookService _bookRepository;

        public BookService(IBookService bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Select(MapToDto);
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book == null ? null : MapToDto(book);
        }

        public async Task<IEnumerable<BookDto>> GetFeaturedAsync()
        {
            var books = await _bookRepository.GetFeaturedAsync();
            return books.Select(MapToDto);
        }



        private static BookDto MapToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                YearPublication = book.YearPublication,
                GenderId = book.GenderId,
                GenderName = book.Gender?.Name ?? string.Empty,
                IsFeatured = book.IsFeatured,
                CreatedAt = book.CreatedAt
            };
        }
    }
}
