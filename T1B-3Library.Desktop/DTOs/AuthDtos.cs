namespace T1B_3Library.Desktop.DTOs
{
    // DTO contendo as informações enviadas pelo formulário para realizar o login
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO contendo as informações enviadas pelo formulário para realizar o cadastro
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        // Campo adicionado para enviar o perfil/nível de acesso selecionado (ex: "Admin", "Reader", "Librarian")
        public string Role { get; set; } = "Reader";
    }

    // DTO contendo o resultado retornado pela API após autenticação ou registro
    public record AuthResponseDto(
        bool Success,       // Indica se a operação deu certo (true/false)
        string Message,     // Mensagem explicativa (ex: "Login efetuado com sucesso")
        string? Username,   // Nome do usuário logado
        string? Role,       // Perfil do usuário autenticado
        string? Token       // Token JWT para autorizar as próximas chamadas HTTP
    );
}