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
    }

    // DTO contendo o resultado retornado pela API após autenticação ou registro
    public class UserResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();

        /// <summary>
        /// Verifica se o usuário possui a role "Admin" e retorna true ou false.
        /// usando controle de acesso na interface
        /// </summary>
        public bool IsAdmin => Roles.Contains("Admin");
    }

}