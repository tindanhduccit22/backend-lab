using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;
using Lab4.DTOs;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly BookManagementContext _context;

        public LoansController(BookManagementContext context)
        {
            _context = context;
        }

        // GET /api/loans: Filter by UserId, Status, or loan date range.
        [HttpGet]
        public async Task<ActionResult> GetLoans(
            [FromQuery] int? userId,
            [FromQuery] int? status,
            [FromQuery] DateTime? loanDate,
            [FromQuery] DateTime? dueDate,
            [FromQuery] DateTime? returnDate)
        {
            var query = _context.Loans
                .Include(l => l.Users)
                .Include(l => l.Book)
                .AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(l => l.UserId == userId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(l => l.Status == status.Value);
            }

            if (loanDate.HasValue)
            {
                query = query.Where(l => l.LoanDate == loanDate.Value);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(l => l.DueDate == dueDate.Value);
            }

            if (returnDate.HasValue)
            {
                query = query.Where(l => l.ReturnDate == returnDate.Value);
            }
            return Ok(await query.ToListAsync());
        }

        // GET /api/loans/{id}: View loan details.
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetLoans(int id)
        {
            var loans = await _context.Loans
                .Include(l => l.Users)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(a => a.LoanId == id);

            if (loans == null) return NotFound("Not Found");

            return Ok(loans);
        }

        // POST: Create new loan record (only if AvailableCopies > 0).
        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDto dto)
        {
            // check xem sinh viên đó có trong hệ thống không
            var checkUser = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!checkUser)
            {
                return BadRequest("The user does not exist");
            }

            // check tìm cuốn sách
            var book = await _context.Books
    .FirstOrDefaultAsync(b => b.BookId == dto.BookId);
            if (book == null)
            {
                return BadRequest("Books do not exist");
            }

            // Kiểm tra số lượng bản sao còn lại
            if (book.AvailableCopies <= 0)
            {
                return BadRequest("Sách này đã hết bản sao có sẵn để mượn!");
            }

            // 4. Dùng Transaction để đảm bảo an toàn khi cập nhật nhiều bảng
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // khi mượn 1 cuốn thì giảm đi 1
                book.AvailableCopies -= 1;
                _context.Books.Update(book);

                // đồng thời ghi lại người mượn mới
                var newLoan = new Loans
                {
                    UserId = dto.UserId,
                    BookId = dto.BookId,
                    LoanDate = DateTime.UtcNow, 
                    DueDate = DateTime.UtcNow.AddDays(14), 
                    Status = 0 // 0: Đang mượn, 1: Đã trả
                };

                await _context.Loans.AddAsync(newLoan);

                // Lưu tất cả thay đổi xuống Database
                await _context.SaveChangesAsync();

                // Xác nhận hoàn thành transaction thành công
                await transaction.CommitAsync();

                // Trả về kết quả 201 Created kèm thông tin phiếu mượn vừa tạo
                return Ok(newLoan);
            }
            catch (Exception ex)
            {
                // Nếu có bất kỳ lỗi gì xảy ra trong khối try, hủy bỏ toàn bộ tác vụ đã làm ở trên
                await transaction.RollbackAsync();
                return StatusCode(500, $"System error when creating a loan slip: {ex.Message}");
            }
        }


        // PUT: Update return date and automatically set Status to Returned.
        [HttpPut("{id}")]
        public async Task<ActionResult<Loans>> ReturnBook(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound(new { message = $"Loan record {id} not found." });
            }

            if (loan.Status == 1)
            {
                return BadRequest(new { message = "This book has already been processed as returned." });
            }

            var book = await _context.Books.FindAsync(loan.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
            }

            loan.ReturnDate = DateTime.Now;
            loan.Status = 1; // 1: Returned

            await _context.SaveChangesAsync();
            return Ok(loan);
        }
    }
}
