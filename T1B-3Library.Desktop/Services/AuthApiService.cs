// =============================================================================
// T1B-3Library.Desktop - Services/AuthApiService.cs
// =============================================================================
// Serviço de Autenticação — padronizado seguindo o exemplo fornecido.
// Endpoints padronizados para "/api/auth/..." e uso do HttpClientHelper.Instance.
// =============================================================================

using System;
using System.Threading.Tasks;
using T1B_3Library.Desktop.DTOs;
using T1B_3Library.Desktop.Helpers;

namespace T1B_3Library.Desktop.Services
{
    /// <summary>
    /// Serviço de comunicação com os endpoints de autenticação da API.
    /// </summary>
    public class AuthApiService
    {
        private readonly HttpClientHelper _http;

        // Construtor sem parâmetros seguindo o padrão do exemplo (uso do singleton)
        public AuthApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        /// <summary>
        /// Realiza o login chamando POST /api/auth/login.
        /// Retorna tupla (Sucesso, User, ErrorMessage).
        /// </summary>
        public async Task<(bool Sucesso, AuthResponseDto? User, string ErrorMessage)> LoginAsync(string email, string password)
        {
            var loginDto = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            var (sucesso, data, error) = await _http.PostAsync<AuthResponseDto>("/api/auth/login", loginDto);

            if (sucesso && data != null && !string.IsNullOrWhiteSpace(data.Token))
            {
                // Armazena usuário e token na sessão local
                SessionManager.StartSession(data, data.Token);
            }

            return (sucesso, data, error);
        }

        /// <summary>
        /// Realiza o logout chamando POST /api/auth/logout.
        /// Limpa cookies e sessão local.
        /// </summary>
        public async Task<(bool Sucesso, string ErrorMessage)> LogoutAsync()
        {
            var result = await _http.PostEmptyAsync("/api/auth/logout");

            // Limpa sessão local independentemente do resultado da API
            SessionManager.ClearSession();
            _http.ClearCookies();

            return result;
        }

        /// <summary>
        /// Busca os dados do usuário autenticado via GET /api/auth/me.
        /// </summary>
        public async Task<AuthResponseDto?> GetCurrentUserAsync()
        {
            return await _http.GetAsync<AuthResponseDto>("/api/auth/me");
        }

        /// <summary>
        /// Registra um novo usuário via POST /api/auth/register.
        /// Retorna tupla (Sucesso, ErrorMessage).
        /// </summary>
        public async Task<(bool Sucesso, string ErrorMessage)> RegisterAsync(string email, string password, string confirmPassword)
        {
            var dto = new RegisterRequestDto
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var (success, _, error) = await _http.PostAsync<object>("/api/auth/register", dto);
            return (success, error);
        }

        /// <summary>
        /// Verifica se existe sessão ativa (token armazenado localmente).
        /// </summary>
        public bool IsAuthenticated() => !string.IsNullOrWhiteSpace(SessionManager.Token);

        /// <summary>
        /// Limpa sessão apenas localmente.
        /// </summary>
        public void ClearLocalSession()
        {
            SessionManager.ClearSession();
            _http.ClearCookies();
        }

        /// <summary>
        /// Alias para verificar sessão ativa.
        /// </summary>
        public bool HasActiveSession() => !string.IsNullOrWhiteSpace(SessionManager.Token);
    }
}

