using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1B_3Library.Application.DTOs;

namespace T1B_3Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto?> GetByIdAsync(int id);
        Task<IEnumerable<BookDto>> GetFeaturedAsync();
        Task<IEnumerable<BookDto>> GetByCategoryAsync(int categoryId);
        Task<BookDto> CreateAsync(CreateBookDto dto);
        Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
