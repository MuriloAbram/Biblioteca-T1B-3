using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T1B_3Library.Application.DTOs;
using T1B_3Library.Application.Interfaces;

namespace T1B_3Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Retorna todos os games.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var book = await _bookService.GetAllAsync();
            return Ok(book);
        }

        // Busca um game específico pelo Id.
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book == null)
                return NotFound(new { message = "Livro não encontrado." });

            return Ok(book);
        }

        // Cria um novo game.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
        {
            var book = await _bookService.CreateAsync(dto);

            // Retorna 201 Created com a URL do recurso criado
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        /// Atualiza um game existente.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDto>> Update(int id, [FromBody] UpdateBookDto dto)
        {
            var book = await _bookService.UpdateAsync(id, dto);

            if (book == null)
                return NotFound(new { message = "Livro não encontrado." });

            return Ok(book);
        }

        // Remove um game.
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _bookService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Livro não encontrado." });

            return NoContent();
        }

    }
}
