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
    /// Serviço de comunicação com os endpoints de autenticação da API
    /// </summary>
    public class AuthApiService
    {
        private readonly HttpClientHelper _http;

        public AuthApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        /// <summary>
        /// Realiza o login chamando POST /api/auth/login.
        ///
        /// O que acontece internamente:
        /// 1. Envia email + senha para a API em formato JSON
        /// 2. A API valida as credenciais com o ASP.NET Core Identity
        /// 3. Se válido, a API retorna um cookie de sessão + dados do usuário
        /// 4. O CookieContainer do HttpClient armazena o cookie automaticamente
        /// 5. Retornamos os dados do usuário para o LoginForm
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <returns>Tupla com sucesso, dados do usuário e mensagem de erro</returns>
        public async Task<(bool Sucesso, UserResponseDto? User, string ErrorMessage)> LoginAsync(string email, string password)
        {
            var loginDto = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            var (sucesso, data, error) = await _http.PostAsync<UserResponseDto>(
                "/api/auth/login", loginDto);

            return (sucesso, data, error);
        }

        /// <summary>
        /// Realiza o logout chamando POST /api/auth/logout.
        /// Também limpa os cookies de sessão localmente
        /// </summary>
        public async Task<(bool Sucesso, string ErrorMessage)> LogoutAsync()
        {
            var result = await _http.PostEmptyAsync("/api/auth/logout");

            _http.ClearCookies();

            return result;
        }

        /// <summary>
        /// Busca os dados do usuário autenticado via GET /api/auth/me.
        /// Útil para verificar se a sessão ainda está ativa
        /// </summary>
        public async Task<UserResponseDto?> GetCurrentUserAsync()
        {
            return await _http.GetAsync<UserResponseDto>("/api/auth/me");
        }

        /// <summary>
        /// Registra um novo usuário via POST /api/auth/register.
        /// </summary>
        public async Task<(bool Sucesso, string ErrorMessage)> RegisterAsync(
            string email, string password, string confirmPassword)
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
    }
}