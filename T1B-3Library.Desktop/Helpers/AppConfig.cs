using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace T1B_3Library.Desktop.Helpers
{
    /// <summary>
    /// Classe responsável por carregar as configurações
    /// do arquivo appsettings.json.
    /// </summary>
    public static class AppConfig
    {
        // Instância da configuração
        private static readonly IConfigurationRoot _configuration;

        // ================================================================
        // CONSTRUTOR
        // ================================================================

        static AppConfig()
        {
            // Diretório onde o executável está sendo executado
            string basePath = AppContext.BaseDirectory;

            // Cria o leitor de configuração
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(
                    "appsettings.json",
                    optional: true,
                    reloadOnChange: true
                );

            // Constrói a configuração
            _configuration = builder.Build();
        }

        // ================================================================
        // URL DA API
        // ================================================================

        /// <summary>
        /// URL base da API.
        /// </summary>
        public static string ApiBaseUrl
        {
            get
            {
                string url =
                    _configuration["ApiSettings:BaseUrl"]
                    ?? "https://localhost:7123/api/";

                url = url.Trim();

                // Garante que termine com /
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }

                return url;
            }
        }

        // ================================================================
        // TIMEOUT
        // ================================================================

        /// <summary>
        /// Tempo máximo das requisições HTTP em segundos.
        /// Valor padrão: 30 segundos.
        /// </summary>
        public static int Timeout
        {
            get
            {
                // Lê o valor diretamente como string,
                // evitando a necessidade do Configuration.Binder.
                string? timeoutValue =
                    _configuration["ApiSettings:Timeout"];

                // Tenta converter para inteiro
                if (int.TryParse(timeoutValue, out int timeout))
                {
                    // Impede valores menores ou iguais a zero
                    if (timeout > 0)
                    {
                        return timeout;
                    }
                }

                // Valor padrão
                return 30;
            }
        }

        // ================================================================
        // MÉTODO AUXILIAR
        // ================================================================

        /// <summary>
        /// Obtém qualquer configuração do appsettings.json.
        ///
        /// Exemplo:
        /// AppConfig.GetValue("ApiSettings:BaseUrl");
        /// </summary>
        public static string? GetValue(string key)
        {
            return _configuration[key];
        }
    }
}

