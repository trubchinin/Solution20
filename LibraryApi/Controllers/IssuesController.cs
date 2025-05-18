using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
using DomainTables;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly LibraryRepository _repo;

        public IssuesController(LibraryRepository repo)
        {
            _repo = repo;
        }

        // GET: api/issues
        [HttpGet]
        public IActionResult GetAll()
        {
            var issues = _repo.GetAllIssues();
            return Ok(issues);
        }

        // GET: api/issues/{readerId}/{bookId}
        [HttpGet("{readerId:int}/{bookId:int}")]
        public IActionResult Get(int readerId, int bookId)
        {
            var issue = _repo.GetIssue(readerId, bookId);
            if (issue == null) return NotFound();
            return Ok(issue);
        }

        // POST: api/issues
        [HttpPost]
        public IActionResult Create([FromBody] IssueDto dto)
        {
            var issue = new Issue
            {
                ReaderId = dto.ReaderId,
                BookId = dto.BookId,
                IssueDate = dto.IssueDate,
                ReturnDate = dto.ReturnDate
            };

            _repo.AddIssue(issue);

            return CreatedAtAction(
                nameof(Get),
                new { readerId = issue.ReaderId, bookId = issue.BookId },
                issue);
        }

        // PUT: api/issues/{readerId}/{bookId}
        [HttpPut("{readerId:int}/{bookId:int}")]
        public IActionResult Update(int readerId, int bookId, [FromBody] IssueDto dto)
        {
            var existing = _repo.GetIssue(readerId, bookId);
            if (existing == null) return NotFound();

            existing.IssueDate = dto.IssueDate;
            existing.ReturnDate = dto.ReturnDate;

            _repo.UpdateIssue(existing);
            return NoContent();
        }

        // DELETE: api/issues/{readerId}/{bookId}
        [HttpDelete("{readerId:int}/{bookId:int}")]
        public IActionResult Delete(int readerId, int bookId)
        {
            var existing = _repo.GetIssue(readerId, bookId);
            if (existing == null) return NotFound();

            _repo.DeleteIssue(readerId, bookId);
            return NoContent();
        }
    }
}