using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly BookManagementContext _context;

    public AuthorController(BookManagementContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors([FromQuery] string? name)
    {
        var query = _context.Authors.Where(a => a.IsActive);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name));

        return Ok(await query.ToListAsync());
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id && a.IsActive);
        if (author == null) return NotFound("Not Found");

        return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor([FromForm] Author author, IFormFile? avatarFile)
    {
        if (avatarFile != null)
        {
            author.Avatar = await SaveFileAsync(avatarFile);
        }

        author.CreatedDate = DateTime.Now;
        author.IsActive = true;

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return Ok(author);
    }

    // PUT 
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuthor(int id, [FromForm] Author author, IFormFile? avatarFile)
    {
        var existing = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id && a.IsActive);
        if (existing == null) return NotFound("No author found to update");

        existing.FirstName = author.FirstName;
        existing.LastName = author.LastName;
        existing.Biography = author.Biography;
        existing.DateOfBirth = author.DateOfBirth;
        existing.Nationality = author.Nationality;
        existing.Email = author.Email;
        existing.Website = author.Website;

        if (avatarFile != null)
            existing.Avatar = await SaveFileAsync(avatarFile);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE 
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null || !author.IsActive) return NotFound("The author does not exist or has been deleted");

        author.IsActive = false;
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