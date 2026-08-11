using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T1B_3Library.Application.DTOs;
using T1B_3Library.Application.Interfaces;

namespace T1B_3Library.API.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    [Authorize]               
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuariosService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
        {
            var (success, usuario, error) = await _usuariosService.CreateAsync(dto);
            if (!success)
                return BadRequest(new { message = error });
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var (success, error) = await _usuariosService.DeleteAsync(id);
            if (!success)
                return BadRequest(new { message = error });
            return NoContent();
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUsuarioDto dto)
        {
            var (success, usuario, error) = await _usuariosService.UpdateAsync(id, dto);
            if (!success)
                return BadRequest(new { message = error });
            return Ok(usuario);
        }
    }
}