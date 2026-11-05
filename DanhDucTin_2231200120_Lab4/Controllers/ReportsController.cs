using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly BookManagementContext _context;

        public ReportsController(BookManagementContext context)
        {
            _context = context;
        }

        [HttpGet ("top-borrowed")]
        public async Task<IActionResult> GetTopBorrowedBooks(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int top = 10)
        {
            var query = _context.Loans
                .Include(b => b.Book)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(f => f.LoanDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(f => f.LoanDate <= toDate.Value);
            }

            var report = await query.GroupBy(l => new { l.BookId, l.Book.Title }).Select(g => new
            {
                BookId = g.Key.BookId,
                Title = g.Key.Title,
                BorrowedCount = g.Count()
            }).OrderByDescending(x => x.BorrowedCount).Take(top).ToListAsync();

            return Ok(report);
        }
    }
}
