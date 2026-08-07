namespace T1B_3Library.Desktop.DTOs
{
    // DTO contendo as informações enviadas pelo formulário para realizar o login
    public record LoginRequestDto(
        string Username,    // Nome de usuário digitado
        string Password     // Senha digitada
    );

    // DTO contendo as informações enviadas pelo formulário para realizar o cadastro
    public record RegisterUserDto(
        string Username,    // Nome de usuário para a nova conta
        string Password,    // Senha para a nova conta
        string Role         // Perfil do usuário (ex: Admin, Reader, Operator)
    );

    // DTO contendo o resultado retornado pela API após autenticação ou registro
    public record AuthResponseDto(
        bool Success,       // Indica se a operação deu certo (true/false)
        string Message,     // Mensagem explicativa (ex: "Login efetuado com sucesso")
        string? Username,   // Nome do usuário logado
        string? Role,       // Perfil do usuário autenticado
        string? Token       // Token JWT para autorizar as próximas chamadas HTTP
    );
}