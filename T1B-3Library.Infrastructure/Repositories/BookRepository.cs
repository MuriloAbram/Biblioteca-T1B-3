using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using T1B_3Library.Domain.Entities;
using T1B_3Library.Domain.Interfaces;
using T1B_3Library.Infrastructure.Context;

namespace T1B_3Library.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Gender)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Gender)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        // Implementação simples para "featured" (ajuste conforme regra de negócio)
        public async Task<IEnumerable<Book>> GetFeaturedAsync()
        {
            return await _context.Books
                .Include(b => b.Gender)
                .OrderByDescending(b => b.Id)
                .Take(5)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByGenderAsync(int genderId)
        {
            return await _context.Books
                .Include(b => b.Gender)
                .Where(b => b.GenderId == genderId)
                .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(Book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book Book)
        {
            _context.Books.Update(Book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Books.CountAsync();
        }
    }
}