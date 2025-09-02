using BooksApp.Services.Interfaces;
using BooksApp.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksApp.API.Controllers
{
    [Route("/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string? userId, [FromQuery] bool? isSold, [FromQuery] string? categoryName)
        {
            if (userId == null && isSold == null && categoryName == null)
            {
                var allBooks = await _bookService.GetAllAsync();
                return Ok(allBooks);
            }
            else
            {
                var filteredBooks = await _bookService.GetBooksByFilters(userId, isSold, categoryName);
                if (filteredBooks.Count == 0)
                    return NoContent();
                return Ok(filteredBooks);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookModel>> GetById(string id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookModel>> Create([FromBody] BookModel bookModel)
        {
            var created = await _bookService.CreateAsync(bookModel);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] BookModel bookModel)
        {
            var existing = await _bookService.GetByIdAsync(bookModel.Id);
            if (existing == null)
                return NotFound();

            await _bookService.UpdateAsync(bookModel);
            return Ok(bookModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _bookService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}
