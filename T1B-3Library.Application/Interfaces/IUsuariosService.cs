using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1B_3Library.Application.DTOs;

namespace T1B_3Library.Application.Interfaces
{
    public interface IUsuariosService
    {
        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto?> GetByIdAsync(string id);
        Task<(bool Success, UsuarioDto? Usuario, string ErrorMessage)> CreateAsync(CreateUsuarioDto dto);
        Task<(bool Success, UsuarioDto? Usuario, string ErrorMessage)> UpdateAsync(string id, UpdateUsuarioDto dto);
        Task<(bool Success, string ErrorMessage)> DeleteAsync(string id);
        Task<IEnumerable<string>> GetPerfisAsync();
    }
}
