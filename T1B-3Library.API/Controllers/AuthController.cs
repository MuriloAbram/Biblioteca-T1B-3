
using Microsoft.AspNetCore.Authorization; // Permite proteger endpoints com autorização
using Microsoft.AspNetCore.Identity; // Recursos do ASP.NET Identity
using Microsoft.AspNetCore.Mvc; // Recursos para criação de Controllers e APIs
using T1B_3Library.Application.DTOs; // Importa os DTOs da aplicação

namespace T1B_3Library.API.Controllers // Define o namespace do Controller
{
    [ApiController] // Indica que esta classe é uma API Controller
    [Route("api/[controller]")] // Define a rota base como /api/Auth
    public class AuthController : ControllerBase // Controller específico para APIs
    {
        private readonly UserManager<IdentityUser> _userManager; // Gerencia os usuários
        private readonly SignInManager<IdentityUser> _signInManager; // Gerencia autenticação e login


        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager; // Recebe o UserManager pelo Dependency Injection
            _signInManager = signInManager; // Recebe o SignInManager pelo Dependency Injection
        }


        // Registra um novo usuário
        [HttpPost("register")] // Endpoint POST /api/Auth/register
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            // Verifica se as senhas informadas são iguais
            if (dto.Password != dto.ConfirmPassword)
            {
                return BadRequest(new
                {
                    message = "As senhas não coincidem."
                });
            }


            // Cria o usuário utilizando o email como nome de usuário
            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };


            // Cria o usuário no banco utilizando o Identity
            var result = await _userManager.CreateAsync(user, dto.Password);


            // Verifica se houve algum erro durante o cadastro
            if (!result.Succeeded)
            {
                // Obtém as mensagens dos erros retornados pelo Identity
                var errors = result.Errors.Select(e => e.Description);

                // Retorna os erros para o cliente
                return BadRequest(new
                {
                    message = "Erro ao registrar.",
                    errors
                });
            }


            // Retorna sucesso após o cadastro
            return Ok(new
            {
                success = true,
                message = "Usuário registrado com sucesso!"
            });
        }


        // Faz login do usuário
        [HttpPost("login")] // Endpoint POST /api/Auth/login
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            // Tenta realizar o login utilizando email e senha
            var result = await _signInManager.PasswordSignInAsync(
                dto.Email,
                dto.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );


            // Verifica se o login não foi realizado
            if (!result.Succeeded)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Email ou senha inválidos."
                });
            }


            // Busca o usuário pelo email
            var user = await _userManager.FindByEmailAsync(dto.Email);


            // Verifica se o usuário foi encontrado
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Usuário não encontrado."
                });
            }


            // Busca as funções/perfis do usuário
            var roles = await _userManager.GetRolesAsync(user);


            // Retorna os dados do usuário
            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }


        // Faz logout do usuário
        [HttpPost("logout")] // Endpoint POST /api/Auth/logout
        [Authorize] // Exige usuário autenticado
        public async Task<ActionResult> Logout()
        {
            // Encerra a sessão do usuário
            await _signInManager.SignOutAsync();

            // Retorna mensagem de sucesso
            return Ok(new
            {
                success = true,
                message = "Logout realizado com sucesso!"
            });
        }


        // Retorna os dados do usuário autenticado
        [HttpGet("me")] // Endpoint GET /api/Auth/me
        [Authorize] // Exige usuário autenticado
        public async Task<ActionResult<UserDto>> Me()
        {
            // Obtém o usuário atualmente autenticado
            var user = await _userManager.GetUserAsync(User);


            // Verifica se o usuário foi encontrado
            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Usuário não autenticado."
                });
            }


            // Busca as funções/perfis do usuário
            var roles = await _userManager.GetRolesAsync(user);


            // Retorna os dados do usuário
            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }
    }
}

