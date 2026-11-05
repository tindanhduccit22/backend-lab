using lab4.Data;
using lab4.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarouselController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] Carousel carousel,
            IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/carousel_images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(imageFile.FileName);

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                carousel.ImageUrl = "/carousel_images/" + fileName;
            }

            carousel.CreatedDate = DateTime.Now;
            carousel.UpdatedDate = DateTime.Now;

            _context.Carousels.Add(carousel);

            await _context.SaveChangesAsync();

            return Ok(carousel);
        }
    }
}