using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1B_3Library.Application.DTOs
{
    // DTO para exibir informações do usuário
    public class UsuarioDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
    // DTO para criar um novo usuário
    public class CreateUsuarioDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Roles { get; set; } = "Usuário";
    }
    // DTO pra atualizar o usuário, incluindo a possibilidade de alterar a senha
    public class UpdateUsuarioDto
    {
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string Roles { get; set; } = string.Empty;
    }
}
