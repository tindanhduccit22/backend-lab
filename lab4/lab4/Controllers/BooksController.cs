using lab4.Data;
using lab4.DTOs;
using lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] BookCreateDto request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                return BadRequest("CategoryId does not exist.");

            var author = await _context.Authors.FindAsync(request.AuthorId);
            if (author == null)
                return BadRequest("AuthorId does not exist.");

            string avatarPath = "";
            string pdfPath = "";

            if (request.AvatarFile != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "book_images");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.AvatarFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.AvatarFile.CopyToAsync(stream);

                avatarPath = "/book_images/" + fileName;
            }

            if (request.PdfFile != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "book_pdfs");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.PdfFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.PdfFile.CopyToAsync(stream);

                pdfPath = "/book_pdfs/" + fileName;
            }

            var book = new Book
            {
                Title = request.Title,
                Description = request.Description,
                BookCode = request.BookCode,
                Publisher = request.Publisher,
                PublishedYear = request.PublishedYear,
                CategoryId = request.CategoryId,
                AuthorId = request.AuthorId,
                TotalCopies = request.TotalCopies,
                AvailableCopies = request.AvailableCopies,
                CreatedDate = DateTime.Now,
                Avatar = avatarPath,
                Pdf = pdfPath,
                IsActive = true,
                IsDeleted = false
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                book.BookId,
                book.Title,
                book.Description,
                book.BookCode,
                book.Publisher,
                book.PublishedYear,
                book.CategoryId,
                book.AuthorId,
                book.TotalCopies,
                book.AvailableCopies,
                book.Avatar,
                book.Pdf,
                book.IsActive,
                book.IsDeleted
            });
        }
    }
}