using System;
using T1B_3Library.Desktop.DTOs;

namespace T1B_3Library.Desktop.Helpers
{
    /// <summary>
    /// Gerencia os dados do usuário autenticado durante a execução da aplicação.
    /// </summary>
    public static class SessionManager
    {
        // ================================================================
        // SUPORTE PARA ACESSO VIA SessionManager.Instance
        // ================================================================
        public static class Instance
        {
            public static AuthResponseDto? CurrentUser => SessionManager.CurrentUser;
            public static string? Token => SessionManager.Token;
            public static bool IsLoggedIn => SessionManager.IsLoggedIn;
            public static bool IsAuthenticated => SessionManager.IsAuthenticated;

            // Adicionado suporte ao IsAdmin via Instance
            public static bool IsAdmin => SessionManager.IsAdmin;

            public static void StartSession(AuthResponseDto user, string token)
                => SessionManager.StartSession(user, token);

            public static void EndSession() => SessionManager.EndSession();
            public static void ClearSession() => SessionManager.ClearSession();
            public static void Logout() => SessionManager.Logout();
        }

        // ================================================================
        // USUÁRIO ATUAL
        // ================================================================
        public static AuthResponseDto? CurrentUser { get; private set; }

        // ================================================================
        // TOKEN JWT
        // ================================================================
        public static string? Token { get; private set; }

        // ================================================================
        // ESTADO DA SESSÃO E PERMISSÕES
        // ================================================================
        public static bool IsLoggedIn => CurrentUser != null && !string.IsNullOrWhiteSpace(Token);

        public static bool IsAuthenticated => IsLoggedIn;

        /// <summary>
        /// Verifica se o usuário atual possui a Role / Perfil de Administrador.
        /// (Ajuste a palavra "Admin" caso sua API use "ADMINISTRATOR" ou outro valor)
        /// </summary>
        public static bool IsAdmin => CurrentUser != null &&
            (CurrentUser.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true ||
             CurrentUser.Role?.Equals("Administrador", StringComparison.OrdinalIgnoreCase) == true);

        // ================================================================
        // INICIAR SESSÃO
        // ================================================================
        public static void StartSession(AuthResponseDto user, string token)
        {
            if (user == null)
            {
                throw new ArgumentNullException(
                    nameof(user),
                    "Os dados do usuário não podem ser nulos."
                );
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(
                    "O token JWT não pode ser vazio.",
                    nameof(token)
                );
            }

            CurrentUser = user;
            Token = token;
        }

        // ================================================================
        // ENCERRAR SESSÃO / CLEAR / LOGOUT
        // ================================================================
        public static void EndSession()
        {
            CurrentUser = null;
            Token = null;
        }

        public static void ClearSession() => EndSession();

        public static void Logout() => EndSession();
    }
}