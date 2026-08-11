using System;
using System.Threading.Tasks;
using T1B_3Library.Desktop.DTOs;
using T1B_3Library.Desktop.Helpers;

namespace T1B_3Library.Desktop.Services
{
    /// <summary>
    /// Serviço responsável pela comunicação com os endpoints
    /// de autenticação da API.
    ///
    /// Responsabilidades:
    /// - Login
    /// - Cadastro
    /// - Logout
    /// - Gerenciamento da sessão
    /// - Armazenamento do JWT através do SessionManager
    /// </summary>
    public class AuthApiService
    {
        // ================================================================
        // CAMPOS
        // ================================================================

        /// <summary>
        /// Helper responsável pelas requisições HTTP.
        /// </summary>
        private readonly HttpClientHelper _http;


        // ================================================================
        // CONSTRUTOR
        // ================================================================

        /// <summary>
        /// Cria uma nova instância do serviço de autenticação.
        /// </summary>
        /// <param name="httpHelper">
        /// Instância do HttpClientHelper.
        /// </param>
        public AuthApiService(HttpClientHelper httpHelper)
        {
            _http = httpHelper
                ?? throw new ArgumentNullException(nameof(httpHelper));
        }


        // ================================================================
        // LOGIN
        // ================================================================

        /// <summary>
        /// Realiza o login do usuário.
        ///
        /// Envia:
        /// POST /api/auth/login
        ///
        /// Caso a API retorne sucesso e um JWT válido,
        /// a sessão do usuário é iniciada.
        /// </summary>
        public async Task<(bool Sucesso, AuthResponseDto? User, string ErrorMessage)> LoginAsync(string email, string password)
        {
            var loginDto = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            var (sucesso, data, error) = await _http.PostAsync<AuthResponseDto>(
                "/api/auth/login", loginDto);

            // ------------------------------------------------------------
            // Inicia sessão (quando a API retornar sucesso e token)
            // ------------------------------------------------------------
            if (sucesso && data != null &&
                !string.IsNullOrWhiteSpace(data.Token))
            {
                // Armazena usuário e token na sessão local
                SessionManager.StartSession(data, data.Token);
            }

            // ------------------------------------------------------------
            // Retorna resultado da chamada
            // ------------------------------------------------------------
            return (sucesso, data, error);
        }


        // ================================================================
        // CADASTRO
        // ================================================================

        /// <summary>
        /// Realiza o cadastro de um novo usuário.
        ///
        /// Envia:
        /// POST /api/auth/register
        /// </summary>
        public async Task<AuthResponseDto?> RegisterAsync(
            RegisterRequestDto registerDto)
        {
            // ------------------------------------------------------------
            // Validação
            // ------------------------------------------------------------

            if (registerDto == null)
            {
                throw new ArgumentNullException(
                    nameof(registerDto)
                );
            }


            // ------------------------------------------------------------
            // Envia cadastro para a API
            // ------------------------------------------------------------

            var result =
                await _http.PostAsync<
                    RegisterRequestDto,
                    AuthResponseDto
                >(
                    "auth/register",
                    registerDto
                );


            // ------------------------------------------------------------
            // Retorna resposta
            // ------------------------------------------------------------

            return result;
        }


        // ================================================================
        // LOGOUT
        // ================================================================

        /// <summary>
        /// Encerra a sessão do usuário.
        ///
        /// Primeiro tenta informar a API sobre o logout.
        /// Depois limpa a sessão local independentemente
        /// do resultado da API.
        /// </summary>
        public async Task<bool> LogoutAsync()
        {
            try
            {
                // --------------------------------------------------------
                // Se não estiver autenticado, apenas limpa localmente
                // --------------------------------------------------------

                if (!IsAuthenticated())
                {
                    SessionManager.ClearSession();
                    _http.ClearCookies();

                    return true;
                }


                // --------------------------------------------------------
                // Informa a API sobre o logout
                // --------------------------------------------------------

                var result =
                    await _http.PostEmptyAsync(
                        "auth/logout"
                    );


                // --------------------------------------------------------
                // Limpa sessão local
                // --------------------------------------------------------

                SessionManager.ClearSession();

                // Remove cookies eventualmente armazenados
                _http.ClearCookies();


                // --------------------------------------------------------
                // Retorna resultado da API
                // --------------------------------------------------------

                return result.Success;
            }
            catch
            {
                // Mesmo que a API esteja indisponível,
                // devemos encerrar a sessão local.

                SessionManager.ClearSession();
                _http.ClearCookies();

                return false;
            }
        }


        // ================================================================
        // VERIFICAR AUTENTICAÇÃO
        // ================================================================

        /// <summary>
        /// Verifica se existe uma sessão autenticada.
        /// </summary>
        public bool IsAuthenticated()
        {
            return !string.IsNullOrWhiteSpace(
                SessionManager.Token
            );
        }


        // ================================================================
        // ENCERRAR SESSÃO LOCAL
        // ================================================================

        /// <summary>
        /// Encerra a sessão apenas no Desktop.
        ///
        /// Útil quando a API não possui endpoint de logout
        /// ou quando ocorre algum erro de comunicação.
        /// </summary>
        public void ClearLocalSession()
        {
            SessionManager.ClearSession();
            _http.ClearCookies();
        }


        // ================================================================
        // VALIDAR SESSÃO
        // ================================================================

        /// <summary>
        /// Verifica se existe um token JWT armazenado.
        ///
        /// Este método não garante que o token ainda seja válido
        /// no servidor. Ele apenas verifica se existe uma sessão local.
        /// </summary>
        public bool HasActiveSession()
        {
            return !string.IsNullOrWhiteSpace(
                SessionManager.Token
            );
        }
    }
}

