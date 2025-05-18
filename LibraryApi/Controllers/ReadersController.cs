using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
using DomainTables;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadersController : ControllerBase
    {
        private readonly LibraryRepository _repo;

        public ReadersController(LibraryRepository repo)
        {
            _repo = repo;
        }

        // GET: api/readers
        [HttpGet]
        public IActionResult GetAll()
        {
            var readers = _repo.GetAllReaders();
            return Ok(readers);
        }

        // GET: api/readers/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var reader = _repo.GetReader(id);
            if (reader == null) return NotFound();
            return Ok(reader);
        }

        // POST: api/readers
        [HttpPost]
        public IActionResult Create([FromBody] Reader reader)
        {
            _repo.AddReader(reader);
            return CreatedAtAction(nameof(Get), new { id = reader.Id }, reader);
        }

        // PUT: api/readers/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Reader reader)
        {
            if (_repo.GetReader(id) == null) return NotFound();
            reader.Id = id;
            _repo.UpdateReader(reader);
            return NoContent();
        }

        // DELETE: api/readers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_repo.GetReader(id) == null) return NotFound();
            _repo.DeleteReader(id);
            return NoContent();
        }
    }
}