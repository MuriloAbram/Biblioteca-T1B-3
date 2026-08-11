using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace T1B_3Library.Desktop.Helpers
{
    /// <summary>
    /// Helper centralizado para comunicação HTTP com a API.
    ///
    /// Responsável por:
    /// - GET
    /// - POST
    /// - PUT
    /// - DELETE
    /// - Autenticação JWT
    /// - Cookies de sessão
    /// - Serialização e desserialização JSON
    /// - Tratamento de erros
    /// - Verificação de disponibilidade da API
    ///
    /// O HttpClient é reutilizado através do padrão Singleton.
    /// </summary>
    public sealed class HttpClientHelper
    {
        // ================================================================
        // SINGLETON
        // ================================================================

        private static readonly Lazy<HttpClientHelper> _instance =
            new(() => new HttpClientHelper());

        /// <summary>
        /// Instância global do HttpClientHelper.
        /// </summary>
        public static HttpClientHelper Instance => _instance.Value;


        // ================================================================
        // CAMPOS PRIVADOS
        // ================================================================

        /// <summary>
        /// Armazena os cookies recebidos pela API.
        /// </summary>
        private readonly CookieContainer _cookieContainer;

        /// <summary>
        /// Handler responsável pelo gerenciamento da comunicação HTTP.
        /// </summary>
        private readonly HttpClientHandler _handler;

        /// <summary>
        /// Cliente HTTP reutilizado pela aplicação.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Configurações utilizadas na serialização JSON.
        /// </summary>
        private readonly JsonSerializerOptions _jsonOptions;


        // ================================================================
        // CONSTRUTOR
        // ================================================================

        private HttpClientHelper()
        {
            // ------------------------------------------------------------
            // Cookie Container
            // ------------------------------------------------------------

            _cookieContainer = new CookieContainer();


            // ------------------------------------------------------------
            // Handler
            // ------------------------------------------------------------

            _handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,

                // Permite que o HttpClient gerencie cookies automaticamente
                UseCookies = true,

                // Evita redirecionamentos automáticos.
                // Assim conseguimos tratar 401/403 corretamente.
                AllowAutoRedirect = false,

                // Aceita certificado HTTPS inválido apenas para desenvolvimento.
                // Em produção, essa configuração deve ser removida.
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };


            // ------------------------------------------------------------
            // URL da API
            // ------------------------------------------------------------

            string baseUrl = AppConfig.ApiBaseUrl;

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException(
                    "A URL da API não foi configurada. " +
                    "Verifique o AppConfig.ApiBaseUrl."
                );
            }

            // Garante que a URL termine com /
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }


            // ------------------------------------------------------------
            // HttpClient
            // ------------------------------------------------------------

            _client = new HttpClient(_handler)
            {
                BaseAddress = new Uri(baseUrl),

                // Tempo máximo de espera de uma requisição
                Timeout = TimeSpan.FromSeconds(
                    AppConfig.Timeout > 0
                        ? AppConfig.Timeout
                        : 30
                )
            };


            // ------------------------------------------------------------
            // Cabeçalhos padrão
            // ------------------------------------------------------------

            _client.DefaultRequestHeaders.Accept.Clear();

            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );


            // ------------------------------------------------------------
            // JSON
            // ------------------------------------------------------------

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }


        // ================================================================
        // AUTENTICAÇÃO
        // ================================================================

        /// <summary>
        /// Adiciona automaticamente o JWT da sessão ao cabeçalho
        /// Authorization da requisição.
        /// </summary>
        private void AttachBearerToken()
        {
            if (!string.IsNullOrWhiteSpace(SessionManager.Token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        SessionManager.Token
                    );
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = null;
            }
        }


        // ================================================================
        // PING DA API
        // ================================================================

        /// <summary>
        /// Verifica se a API está funcionando.
        /// </summary>
        public async Task<(bool IsAvailable, string ErrorMessage)> PingApiAsync()
        {
            try
            {
                using var cts =
                    new CancellationTokenSource(
                        TimeSpan.FromSeconds(5)
                    );

                // Endpoint público da aplicação.
                // Caso sua API tenha outro endpoint público,
                // altere aqui.
                var response = await _client.GetAsync(
                    "api/books",
                    cts.Token
                );

                // Qualquer resposta HTTP significa que a API respondeu.
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (
                    false,
                    CategorizeConnectionError(
                        ex,
                        _client.BaseAddress?.ToString() ?? ""
                    )
                );
            }
        }


        // ================================================================
        // GET
        // ================================================================

        /// <summary>
        /// Executa uma requisição GET e converte a resposta para T.
        /// </summary>
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                AttachBearerToken();

                var response =
                    await _client.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                var responseContent =
                    await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(
                    responseContent,
                    _jsonOptions
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[GET] Erro em {endpoint}: {ex.Message}"
                );

                throw;
            }
        }


        // ================================================================
        // POST - REQUEST + RESPONSE
        // ================================================================

        /// <summary>
        /// Executa POST enviando um objeto e recebendo outro objeto.
        ///
        /// Exemplo:
        ///
        /// PostAsync<LoginRequestDto, AuthResponseDto>()
        /// </summary>
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest data)
        {
            try
            {
                AttachBearerToken();

                string json =
                    JsonSerializer.Serialize(
                        data,
                        _jsonOptions
                    );

                using var body =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );

                var response =
                    await _client.PostAsync(
                        endpoint,
                        body
                    );

                string responseContent =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[POST] {endpoint} -> " +
                        $"{(int)response.StatusCode} " +
                        $"{response.ReasonPhrase}\n" +
                        responseContent
                    );

                    return default;
                }

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return default;
                }

                try
                {
                    return JsonSerializer.Deserialize<TResponse>(
                        responseContent,
                        _jsonOptions
                    );
                }
                catch (JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[POST] Erro ao converter JSON: {ex.Message}"
                    );

                    return default;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[POST] Erro em {endpoint}: {ex.Message}"
                );

                throw;
            }
        }


        // ================================================================
        // POST - RESPONSE
        // ================================================================

        /// <summary>
        /// Executa POST enviando qualquer objeto e recebendo T.
        ///
        /// Retorna uma tupla com:
        /// - Success
        /// - Data
        /// - ErrorMessage
        /// </summary>
        public async Task<(bool Success, T? Data, string ErrorMessage)>
            PostAsync<T>(
                string endpoint,
                object body)
        {
            try
            {
                AttachBearerToken();

                string json =
                    JsonSerializer.Serialize(
                        body,
                        _jsonOptions
                    );

                using var content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );

                var response =
                    await _client.PostAsync(
                        endpoint,
                        content
                    );

                string responseBody =
                    await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        return (
                            true,
                            default,
                            string.Empty
                        );
                    }

                    try
                    {
                        var data =
                            JsonSerializer.Deserialize<T>(
                                responseBody,
                                _jsonOptions
                            );

                        return (
                            true,
                            data,
                            string.Empty
                        );
                    }
                    catch (JsonException ex)
                    {
                        return (
                            false,
                            default,
                            $"Erro ao interpretar resposta da API: {ex.Message}"
                        );
                    }
                }

                return (
                    false,
                    default,
                    TryExtractErrorMessage(responseBody)
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    default,
                    CategorizeConnectionError(
                        ex,
                        endpoint
                    )
                );
            }
        }


        // ================================================================
        // POST SEM CORPO
        // ================================================================

        /// <summary>
        /// Executa um POST sem enviar conteúdo.
        ///
        /// Útil para logout e outras ações simples.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)>
            PostEmptyAsync(
                string endpoint)
        {
            try
            {
                AttachBearerToken();

                var response =
                    await _client.PostAsync(
                        endpoint,
                        null
                    );

                if (response.IsSuccessStatusCode)
                {
                    return (
                        true,
                        string.Empty
                    );
                }

                string body =
                    await response.Content.ReadAsStringAsync();

                return (
                    false,
                    TryExtractErrorMessage(body)
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    CategorizeConnectionError(
                        ex,
                        endpoint
                    )
                );
            }
        }


        // ================================================================
        // PUT - REQUEST + RESPONSE
        // ================================================================

        /// <summary>
        /// Executa PUT enviando um objeto e recebendo outro objeto.
        /// </summary>
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(
            string endpoint,
            TRequest data)
        {
            try
            {
                AttachBearerToken();

                string json =
                    JsonSerializer.Serialize(
                        data,
                        _jsonOptions
                    );

                using var body =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );

                var response =
                    await _client.PutAsync(
                        endpoint,
                        body
                    );

                string responseContent =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return default;
                }

                return JsonSerializer.Deserialize<TResponse>(
                    responseContent,
                    _jsonOptions
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[PUT] Erro em {endpoint}: {ex.Message}"
                );

                throw;
            }
        }


        // ================================================================
        // PUT - SOMENTE BOOL
        // ================================================================

        /// <summary>
        /// Executa PUT e retorna apenas true/false.
        ///
        /// Compatível com o código antigo:
        ///
        /// bool sucesso = await http.PutAsync(
        ///     "api/books/1",
        ///     livro
        /// );
        /// </summary>
        public async Task<bool> PutAsync<TRequest>(
            string endpoint,
            TRequest data)
        {
            try
            {
                AttachBearerToken();

                string json =
                    JsonSerializer.Serialize(
                        data,
                        _jsonOptions
                    );

                using var body =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );

                var response =
                    await _client.PutAsync(
                        endpoint,
                        body
                    );

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[PUT] Erro em {endpoint}: {ex.Message}"
                );

                return false;
            }
        }


        // ================================================================
        // DELETE
        // ================================================================

        /// <summary>
        /// Executa DELETE e retorna sucesso ou erro.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)>
            DeleteAsync(
                string endpoint)
        {
            try
            {
                AttachBearerToken();

                var response =
                    await _client.DeleteAsync(
                        endpoint
                    );

                if (response.IsSuccessStatusCode)
                {
                    return (
                        true,
                        string.Empty
                    );
                }

                string body =
                    await response.Content.ReadAsStringAsync();

                return (
                    false,
                    TryExtractErrorMessage(body)
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    CategorizeConnectionError(
                        ex,
                        endpoint
                    )
                );
            }
        }


        // ================================================================
        // LIMPAR COOKIES
        // ================================================================

        /// <summary>
        /// Remove os cookies armazenados pelo HttpClient.
        ///
        /// O JWT também é removido do header.
        /// </summary>
        public void ClearCookies()
        {
            var baseUri =
                _client.BaseAddress;

            if (baseUri != null)
            {
                var cookies =
                    _cookieContainer.GetCookies(
                        baseUri
                    );

                foreach (Cookie cookie in cookies)
                {
                    cookie.Expired = true;
                }
            }

            _client.DefaultRequestHeaders.Authorization = null;
        }


        // ================================================================
        // EXTRAÇÃO DE ERROS
        // ================================================================

        /// <summary>
        /// Tenta encontrar uma mensagem amigável
        /// dentro da resposta JSON da API.
        /// </summary>
        private string TryExtractErrorMessage(
            string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return "Erro desconhecido.";
            }

            try
            {
                using JsonDocument document =
                    JsonDocument.Parse(json);

                JsonElement root =
                    document.RootElement;

                // message
                if (root.TryGetProperty(
                        "message",
                        out JsonElement message))
                {
                    return message.GetString()
                           ?? "Erro desconhecido.";
                }

                // title
                if (root.TryGetProperty(
                        "title",
                        out JsonElement title))
                {
                    return title.GetString()
                           ?? "Erro desconhecido.";
                }

                // error
                if (root.TryGetProperty(
                        "error",
                        out JsonElement error))
                {
                    return error.GetString()
                           ?? "Erro desconhecido.";
                }

                // detail
                if (root.TryGetProperty(
                        "detail",
                        out JsonElement detail))
                {
                    return detail.GetString()
                           ?? "Erro desconhecido.";
                }
            }
            catch
            {
                // Se não for JSON, retorna o texto original.
            }

            return json;
        }


        // ================================================================
        // TRATAMENTO DE ERROS DE CONEXÃO
        // ================================================================

        /// <summary>
        /// Converte erros técnicos em mensagens mais fáceis
        /// para o usuário entender.
        /// </summary>
        private string CategorizeConnectionError(
            Exception ex,
            string endpoint)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[HttpClientHelper] " +
                $"Erro em '{endpoint}': " +
                $"{ex.GetType().Name} - " +
                $"{ex.Message}"
            );


            // ------------------------------------------------------------
            // TIMEOUT
            // ------------------------------------------------------------

            if (ex is TaskCanceledException ||
                ex is OperationCanceledException)
            {
                return
                    "A requisição excedeu o tempo limite.\n\n" +
                    "Verifique se a API está funcionando normalmente.";
            }


            // ------------------------------------------------------------
            // HTTP REQUEST
            // ------------------------------------------------------------

            if (ex is HttpRequestException httpException)
            {
                string message =
                    httpException.Message.ToLowerInvariant();


                // --------------------------------------------------------
                // CONEXÃO RECUSADA
                // --------------------------------------------------------

                if (message.Contains("connection refused") ||
                    message.Contains("actively refused") ||
                    message.Contains("no connection could be made"))
                {
                    return
                        "A API não está em execução.\n\n" +
                        $"URL configurada: {_client.BaseAddress}\n\n" +
                        "Verifique se o projeto " +
                        "T1B_3Library.API está rodando.";
                }


                // --------------------------------------------------------
                // SSL
                // --------------------------------------------------------

                if (message.Contains("ssl") ||
                    message.Contains("certificate") ||
                    message.Contains("https"))
                {
                    return
                        "Erro de conexão SSL.\n\n" +
                        "Verifique o certificado HTTPS da API " +
                        "ou tente utilizar o perfil HTTP.";
                }


                // --------------------------------------------------------
                // DNS
                // --------------------------------------------------------

                if (message.Contains("name or service not known") ||
                    message.Contains("no such host") ||
                    message.Contains("getaddrinfo"))
                {
                    return
                        "Host da API não encontrado.\n\n" +
                        $"URL: {_client.BaseAddress}";
                }


                return
                    $"Erro de comunicação com a API:\n" +
                    $"{httpException.Message}";
            }


            // ------------------------------------------------------------
            // URI INVÁLIDA
            // ------------------------------------------------------------

            if (ex is UriFormatException ||
                ex is InvalidOperationException)
            {
                return
                    "A URL da API é inválida.\n\n" +
                    "Verifique o AppConfig.ApiBaseUrl.";
            }


            // ------------------------------------------------------------
            // ERRO GENÉRICO
            // ------------------------------------------------------------

            return
                $"Erro inesperado:\n{ex.Message}";
        }
    }
}

