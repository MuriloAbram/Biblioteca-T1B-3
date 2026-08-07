using T1B_3Library.Desktop.DTOs; // Importa as estruturas de dados (DTOs)

namespace T1B_3Library.Desktop.Helpers
{
    // Classe estática responsável por manter os dados do usuário autenticado enquanto a aplicação estiver aberta
    public static class SessionManager
    {
        // Propriedade que armazena as informações do usuário atual (Nome, Perfil, etc.)
        public static AuthResponseDto? CurrentUser { get; private set; }

        // Propriedade que armazena o Token JWT para autorização nas chamadas HTTP
        public static string? Token { get; private set; }

        // Propriedade booleana que verifica rapidamente se existe um usuário logado na sessão
        public static bool IsLoggedIn => CurrentUser != null && !string.IsNullOrEmpty(Token);

        // Método chamado no Login bem-sucedido para registrar a sessão ativa
        public static void StartSession(AuthResponseDto user, string token)
        {
            CurrentUser = user; // Guarda a resposta do login
            Token = token; // Guarda o token JWT
        }

        // Método chamado no Logout para limpar os dados da memória
        public static void EndSession()
        {
            CurrentUser = null; // Zera as informações do usuário
            Token = null; // Zera o token JWT
        }
    }
}