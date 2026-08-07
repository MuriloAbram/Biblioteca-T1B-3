using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T1B_3Library.Application.DTOs;
using T1B_3Library.Application.Interfaces;

namespace T1B_3Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GendersController : Controller
    {
        private readonly IGenderService _genderService;

        public GendersController(IGenderService categoryService)
        {
            _genderService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenderDto>>> GetAll()
        {
            var gender = await _genderService.GetAllAsync();
            return Ok(gender);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GenderDto>> Create([FromBody] CreateGenderDto dto)
        {
            var gender = await _genderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = gender.Id }, gender);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GenderDto>> Update(int id, [FromBody] UpdateGenderDto dto)
        {
            var gender = await _genderService.UpdateAsync(id, dto);

            if (gender == null)
                return NotFound(new { message = "Genero não encontrada." });

            return Ok(gender);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _genderService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Genero não encontrada." });

            return NoContent();
        }
    }
}
