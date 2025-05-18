using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
using DomainTables;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryRepository _repo;

        public BooksController(LibraryRepository repo)
        {
            _repo = repo;
        }

        // GET: api/books
        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _repo.GetAllBooks();
            return Ok(books);
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var book = _repo.GetBook(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            _repo.AddBook(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            if (_repo.GetBook(id) == null) return NotFound();
            book.Id = id;
            _repo.UpdateBook(book);
            return NoContent();
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_repo.GetBook(id) == null) return NotFound();
            _repo.DeleteBook(id);
            return NoContent();
        }
    }
}