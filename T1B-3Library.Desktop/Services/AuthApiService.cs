using System.Threading.Tasks; // Importa suporte para execução assíncrona (Task)
using T1B_3Library.Desktop.DTOs; // Importa os DTOs de autenticação
using T1B_3Library.Desktop.Helpers; // Importa o HttpClientHelper e SessionManager

namespace T1B_3Library.Desktop.Services
{
    // Serviço responsável pela comunicação HTTP com os endpoints de Autenticação e Registro
    public class AuthApiService
    {
        // Instância privada do cliente HTTP utilitário
        private readonly HttpClientHelper _httpHelper;

        // Construtor que recebe a instância de HttpClientHelper
        public AuthApiService(HttpClientHelper httpHelper)
        {
            _httpHelper = httpHelper; // Armazena a dependência do cliente HTTP
        }

        // Envia as credenciais para o endpoint de login e inicia a sessão em caso de sucesso
        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginDto)
        {
            // Faz a chamada POST para o endpoint "auth/login" enviando os dados do login
            var result = await _httpHelper.PostAsync<LoginRequestDto, AuthResponseDto>("auth/login", loginDto);

            // Se o login for válido, obtiver sucesso e retornar um token JWT, salva na sessão ativa
            if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
            {
                SessionManager.StartSession(result, result.Token); // Inicia a sessão com os dados do usuário e token
            }

            return result; // Retorna a resposta completa da API
        }

        // Envia os dados de um novo usuário para o endpoint de cadastro
        public async Task<AuthResponseDto?> RegisterAsync(RegisterUserDto registerDto)
        {
            // Faz a chamada POST para o endpoint "auth/register" enviando os dados de cadastro
            return await _httpHelper.PostAsync<RegisterUserDto, AuthResponseDto>("auth/register", registerDto);
        }
    }
}