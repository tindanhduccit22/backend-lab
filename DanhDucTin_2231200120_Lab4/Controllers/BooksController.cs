using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookManagementContext _context;
        private readonly string _carouselImageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "carousel_images");

        public BooksController(BookManagementContext context)
        {
            _context = context;
        }

        // GET: Retrieve a list of books with pagination, search by title, and filtering by CategoryId or AuthorId.
        [HttpGet]
        public async Task<ActionResult> GetBooks(
            [FromQuery] string? title,
            [FromQuery] int? categoryId,
            [FromQuery] int? authorId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Where(b => b.IsActive == true)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            var totalRecords = await query.CountAsync();

            var book = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = book
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult> PostBook([FromForm] BookInputModel input)
        {
            string avatarPath = "";
            string pdfPath = "";

            var uploadFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "carousel_images");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            if (input.AvatarFile != null && input.AvatarFile.Length > 0)
            {
                var avatarFileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(input.AvatarFile.FileName);

                var fullPath = Path.Combine(uploadFolder, avatarFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await input.AvatarFile.CopyToAsync(stream);
                }

                avatarPath = $"/carousel_images/{avatarFileName}";
            }

            if (input.PdfFile != null && input.PdfFile.Length > 0)
            {
                var pdfFileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(input.PdfFile.FileName);

                var fullPath = Path.Combine(uploadFolder, pdfFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await input.PdfFile.CopyToAsync(stream);
                }

                pdfPath = $"/carousel_images/{pdfFileName}";
            }

            var book = new Book
            {
                Title = input.Title,
                Description = input.Description,
                PublishedYear = input.PublishedYear,
                CategoryId = input.CategoryId,
                AuthorId = input.AuthorId,
                TotalCopies = input.TotalCopies,
                AvailableCopies = input.AvailableCopies,
                Avatar = avatarPath,
                Pdf = pdfPath,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Publisher = book.Publisher;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.CategoryId = book.CategoryId;
            existingBook.AuthorId = book.AuthorId;
            existingBook.TotalCopies = book.TotalCopies;
            existingBook.AvailableCopies = book.AvailableCopies;

            _context.Entry(existingBook).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            existingBook.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Book with ID {id} has been deleted successfully."
            });
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        public class BookInputModel
        {
            public string Title { get; set; }
            public string? Description { get; set; }
            public DateTime PublishedYear { get; set; }
            public int CategoryId { get; set; }
            public int AuthorId { get; set; }
            public int TotalCopies { get; set; }
            public int AvailableCopies { get; set; }
            public IFormFile? AvatarFile { get; set; }
            public IFormFile? PdfFile { get; set; }
        }

        [HttpGet("{id}/read")]
        public async Task<IActionResult> PDF(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            if (string.IsNullOrEmpty(book.Pdf))
            {
                return NotFound(new { message = $"PDF {id} not found." });
            }

            return Ok(new
            {
                bookId = book.BookId,
                bookTitle = book.Title,
                pdfURL = book.Pdf
            });
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedBooks()
        {
            var deletedBooks = await _context.Books
                .Where(b => b.IsActive == false)
                .ToListAsync();

            return Ok(deletedBooks);
        }

        [HttpPatch("{id}/restore")]
        public async Task<ActionResult> RestoreBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            if (book.IsActive == true)
            {
                return BadRequest(new
                {
                    message = "This book is currently still functioning normally"
                });
            }

            book.IsActive = true;

            _context.Entry(book).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"The book has been successfully restored: {book.Title}."
            });
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            string wwwRootPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");

            if (!string.IsNullOrEmpty(book.Avatar))
            {
                string avatarPhysicalPath = Path.Combine(
                    wwwRootPath,
                    book.Avatar.TrimStart('/'));

                if (System.IO.File.Exists(avatarPhysicalPath))
                {
                    System.IO.File.Delete(avatarPhysicalPath);
                }
            }

            if (!string.IsNullOrEmpty(book.Pdf))
            {
                string pdfPhysicalPath = Path.Combine(
                    wwwRootPath,
                    book.Pdf.TrimStart('/'));

                if (System.IO.File.Exists(pdfPhysicalPath))
                {
                    System.IO.File.Delete(pdfPhysicalPath);
                }
            }

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted successfully" });
        }
    }
}