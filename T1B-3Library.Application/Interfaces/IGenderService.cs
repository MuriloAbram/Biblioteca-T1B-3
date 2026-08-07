using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1B_3Library.Application.DTOs;

namespace T1B_3Library.Application.Interfaces
{
    public interface IGenderService
    {
        Task<IEnumerable<GenderDto>> GetAllAsync();
        Task<GenderDto?> GetByIdAsync(int id);
        Task<GenderDto> CreateAsync(CreateGenderDto dto);
        Task<GenderDto?> UpdateAsync(int id, UpdateGenderDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
