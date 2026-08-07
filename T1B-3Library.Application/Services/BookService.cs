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

        public async Task<IEnumerable<BookDto>> GetByGenderAsync(int genderId)
        {
            var books = await _bookRepository.GetByGenderAsync(genderId);
            return books.Select(MapToDto);
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Publisher = dto.Publisher,
                YearPublication = dto.YearPublication,
                GenderId = dto.GenderId,
                IsFeatured = dto.IsFeatured,
                CreatedAt = DateTime.Now
            };

            await _bookRepository.AddAsync(book);

            return MapToDto(book);
        }

        public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) 
                return null;

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Publisher = dto.Publisher;
            book.YearPublication = dto.YearPublication;
            book.GenderId = dto.GenderId;
            book.IsFeatured = dto.IsFeatured;

            await _bookRepository.UpdateAsync(book);
            return MapToDto(book);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _bookRepository.GetByIdAsync(id);
            if (game == null) return false;

            await _bookRepository.DeleteAsync(id);
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await _bookRepository.CountAsync();
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
