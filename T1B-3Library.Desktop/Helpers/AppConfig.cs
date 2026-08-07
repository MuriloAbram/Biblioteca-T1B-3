using System.IO; 
using Microsoft.Extensions.Configuration; 

namespace T1B_3Library.Desktop.Helpers
{
    // Classe estática responsável por carregar e disponibilizar as configurações do appsettings.json
    public static class AppConfig
    {
        // Instância privada da interface de configuração do .NET
        private static readonly IConfigurationRoot _configuration;

        // Construtor estático executado automaticamente na primeira vez que a classe for acessada
        static AppConfig()
        {
            // Cria o leitor de configurações apontando para a pasta onde o executável está rodando
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Define o diretório base do aplicativo
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); // Adiciona o arquivo appsettings.json

            // Constrói a árvore de configurações
            _configuration = builder.Build();
        }

        // Propriedade que lê a URL base da API (retorna valor padrão caso não encontre no arquivo JSON)
        public static string ApiBaseUrl => _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7123/api/";
    }
}