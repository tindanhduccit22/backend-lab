using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselsController : ControllerBase
    {
        private readonly BookManagementContext _context;

        public CarouselsController(BookManagementContext context)
        {
            _context = context;
        }

        // GET /api/carousel – Fetch all (or only active) carousel items.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carousel>>> GetCarousels()
        {
            return await _context.Carousels
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.Order) 
                .ToListAsync();
        }

        // GET /api/carousel/{id}- Get carousel item by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Carousel>> GetCarousel(int id)
        {
            var carousel = await _context.Carousels.FindAsync(id);

            if (carousel == null)
            {
                return NotFound(new { message = $"Carousel with ID {id} not found." });
            }

            return carousel;
        }


        // POST /api/carousel – Add a new carousel item.
        [HttpPost]
        public async Task<ActionResult<Carousel>> PostCarousel(Carousel carousel)
        {
            if (carousel.CreatedDate == default)
            {
                carousel.CreatedDate = System.DateTime.Now;
            }

            _context.Carousels.Add(carousel);
            await _context.SaveChangesAsync();

            return Ok(carousel);
        }

        // PUT /api/carousel/{id} – Update an existing item.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCarousel(int id, Carousel carousel)
        {
            if (id != carousel.CarouselId)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var existingCarousel = await _context.Carousels.FindAsync(id);
            if (existingCarousel == null)
            {
                return NotFound(new { message = $"Carousel with ID {id} not found." });
            }

            existingCarousel.Title = carousel.Title;
            existingCarousel.Description = carousel.Description;
            existingCarousel.ImageUrl = carousel.ImageUrl;
            existingCarousel.LinkUrl = carousel.LinkUrl;
            existingCarousel.Order = carousel.Order;
            existingCarousel.IsActive = carousel.IsActive;
            existingCarousel.UpdatedDate = System.DateTime.Now;

            _context.Entry(existingCarousel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarouselExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE /api/carousel/{id} – Remove an item. ( Deleted both of database and file in server )
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCarousel(int id)
        {
            var carousel = await _context.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return NotFound(new { message = $"Carousel with ID {id} not found." });
            }

            _context.Carousels.Remove(carousel);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Carousel with ID {id} has been deleted successfully." });
        }

        private bool CarouselExists(int id)
        {
            return _context.Carousels.Any(e => e.CarouselId == id);
        }
    }
}
