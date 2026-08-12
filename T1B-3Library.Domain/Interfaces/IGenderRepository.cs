using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1B_3Library.Domain.Entities;

namespace T1B_3Library.Domain.Interfaces
{
    public interface IGenderRepository
    {
        Task<IEnumerable<Gender>> GetAllAsync();
        Task<Gender?> GetByIdAsync(int id);
        Task AddAsync(Gender gender);
        Task UpdateAsync(Gender gender);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
