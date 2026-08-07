using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using T1B_3Library.Domain.Entities;
using T1B_3Library.Domain.Interfaces;
using T1B_3Library.Infrastructure.Context;

namespace T1B_3Library.Infrastructure.Repositories
{
    public class GenderRepository : IGenderRepository
    {
        private readonly AppDbContext _context;

        public GenderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gender>> GetAllAsync()
        {
            return await _context.Genders
                .Include(g => g.Books)
                .ToListAsync();
        }

        public async Task<Gender?> GetByIdAsync(int id)
        {
            return await _context.Genders
                .Include(g => g.Books)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Gender gender)
        {
            await _context.Genders.AddAsync(gender);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Gender gender)
        {
            _context.Genders.Update(gender);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var gender = await _context.Genders.FindAsync(id);
            if (gender != null)
            {
                _context.Genders.Remove(gender);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Genders.CountAsync();
        }
    }
}