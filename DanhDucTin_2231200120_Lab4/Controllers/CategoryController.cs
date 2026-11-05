using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BookManagementContext _context;

        public CategoryController(BookManagementContext context)
        {
            _context = context;
        }

        // GET /api/categories: Filter by IsActive and search by name.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory([FromQuery] string? name)
        {
            var query = _context.Categories.Where(c => c.IsActive);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(a => a.Name.Contains(name));

            return Ok(await query.ToListAsync());
        }

        // POST: Add 
        [HttpPost]
        public async Task<ActionResult<Category>> CreateAuthor([FromForm] Category category, IFormFile? avatarFile)
        {
            if (avatarFile != null)
            {
                category.Avatar = await SaveFileAsync(avatarFile);
            }

            category.CreatedDate = DateTime.Now;
            category.IsActive = true;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        // PUT 
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromForm] Category category, IFormFile? avatarFile)
        {
            var existing = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id && c.IsActive);
            if (existing == null) return NotFound("No category found to update");

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.UpdatedDate = category.UpdatedDate;

            if (avatarFile != null)
                existing.Avatar = await SaveFileAsync(avatarFile);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH /api/categories/{id}/toggle: Toggle the IsActive status of a category.
        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult> PatchCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("The category does not exist");
            }

            category.IsActive = !category.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "carousel_images");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/carousel_images/{fileName}";
        }
    }
}
