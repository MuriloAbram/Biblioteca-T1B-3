using System; // Importa tipos primitivos do C#
using System.Net.Http; // Importa suporte para enviar/receber requisições HTTP
using System.Net.Http.Headers; // Importa gerenciador de cabeçalhos HTTP (Authorization, Content-Type)
using System.Text; // Importa suporte para codificação de texto (UTF8)
using System.Text.Json; // Importa suporte para conversão de e para JSON
using System.Threading.Tasks; // Importa suporte a métodos assíncronos (async/await)

namespace T1B_3Library.Desktop.Helpers
{
    // Classe responsável por centralizar todas as requisições HTTP para a API/Backend
    public class HttpClientHelper
    {
        // Instância única do cliente HTTP para reaproveitamento de conexões
        private readonly HttpClient _client;

        // Configurações padrão do conversor JSON (evita problemas com letras maiúsculas/minúsculas)
        private readonly JsonSerializerOptions _jsonOptions;

        // Construtor que inicializa a URL base e as opções de conversão
        public HttpClientHelper()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.ApiBaseUrl) // Define a URL da API vinda do AppConfig
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Permite ler "title" como "Title" sem dar erro
            };
        }

        // Método privado que anexa o token de autenticação (JWT) no cabeçalho se houver sessão ativa
        private void AttachBearerToken()
        {
            if (!string.IsNullOrEmpty(SessionManager.Token)) // Verifica se há token salvo
            {
                // Adiciona o cabeçalho "Authorization: Bearer <token>" na requisição
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);
            }
        }

        // Executa uma requisição GET assíncrona e converte a resposta JSON no objeto do tipo T
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            AttachBearerToken(); // Adiciona o token na requisição
            var response = await _client.GetAsync(endpoint); // Faz o disparo HTTP GET
            response.EnsureSuccessStatusCode(); // Dispara erro se a resposta não for da família 200 (Sucesso)

            var content = await response.Content.ReadAsStringAsync(); // Lê a resposta em formato texto
            return JsonSerializer.Deserialize<T>(content, _jsonOptions); // Transforma o texto JSON no objeto C#
        }

        // Executa uma requisição POST assíncrona enviando dados JSON e recebendo um resultado convertido
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            AttachBearerToken(); // Adiciona o token na requisição
            var jsonContent = JsonSerializer.Serialize(data, _jsonOptions); // Converte o objeto C# para texto JSON
            var body = new StringContent(jsonContent, Encoding.UTF8, "application/json"); // Prepara o corpo do envio HTTP

            var response = await _client.PostAsync(endpoint, body); // Faz o disparo HTTP POST
            var responseContent = await response.Content.ReadAsStringAsync(); // Lê a resposta retornada pela API

            return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions); // Converte a resposta em objeto C#
        }

        // Executa uma requisição PUT assíncrona para atualizar dados no backend
        public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest data)
        {
            AttachBearerToken(); // Adiciona o token na requisição
            var jsonContent = JsonSerializer.Serialize(data, _jsonOptions); // Converte o objeto para JSON
            var body = new StringContent(jsonContent, Encoding.UTF8, "application/json"); // Monta o corpo da requisição

            var response = await _client.PutAsync(endpoint, body); // Faz o disparo HTTP PUT
            return response.IsSuccessStatusCode; // Retorna true se a API responder com código de sucesso
        }

        // Executa uma requisição DELETE assíncrona para apagar registros no backend
        public async Task<bool> DeleteAsync(string endpoint)
        {
            AttachBearerToken(); // Adiciona o token na requisição
            var response = await _client.DeleteAsync(endpoint); // Faz o disparo HTTP DELETE
            return response.IsSuccessStatusCode; // Retorna true se a exclusão foi concluída com sucesso
        }
    }
}