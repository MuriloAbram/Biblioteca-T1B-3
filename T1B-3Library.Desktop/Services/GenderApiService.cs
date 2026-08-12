using T1B_3Library.Desktop.DTOs;
using T1B_3Library.Desktop.Helpers;

namespace T1B_3Library.Desktop.Services
{
    /// <summary>
    /// Serviço de comunicação com os endpoints de Categorias da API.
    /// </summary>
    public class GenderApiService
    {
        private readonly HttpClientHelper _http;

        public GenderApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        /// <summary>
        /// Lista todas as categorias via GET /api/categories.
        /// </summary>
        public async Task<List<GenderResponseDto>> GetAllAsync()
        {
            try
            {
                var gender = await _http.GetAsync<List<GenderResponseDto>>("/api/gender");
                return gender ?? new List<GenderResponseDto>();
            }
            catch
            {
                return new List<GenderResponseDto>();
            }
        }

        /// <summary>
        /// Cria uma nova categoria via POST /api/categories.
        /// Requer perfil Admin.
        /// </summary>
        public async Task<(bool Success, GenderResponseDto? Gender, string ErrorMessage)>
            CreateAsync(CreateGenderDto dto)
        {
            return await _http.PostAsync<GenderResponseDto>("/api/gender", dto);
        }

        /// <summary>
        /// Atualiza uma categoria via PUT /api/categories/{id}.
        /// Requer perfil Admin.
        /// </summary>
        public async Task<(bool Success, GenderResponseDto? Gender, string ErrorMessage)>
            UpdateAsync(int id, UpdateGenderDto dto)
        {
            return await _http.PutAsync<GenderResponseDto>($"/api/gender/{id}", dto);
        }

        /// <summary>
        /// Exclui uma categoria via DELETE /api/categories/{id}.
        /// Requer perfil Admin.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> DeleteAsync(int id)
        {
            return await _http.DeleteAsync($"/api/gender/{id}");
        }
    }
}
