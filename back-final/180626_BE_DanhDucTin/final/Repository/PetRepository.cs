using final.Data;
using final.Models;
using final.Repositories;
using Microsoft.EntityFrameworkCore;

namespace final.Repository
{
    public class PetRepository : IPetRepository
    {
        private readonly AppDbContext _context;

        public PetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pet>> GetAll()
        {
            return await _context.Pets.ToListAsync();
        }

        public async Task<Pet?> GetById(int id)
        {
            return await _context.Pets.FindAsync(id);
        }

        public async Task Add(Pet pet )
        {
            await _context.Pets.AddAsync(pet);


            await _context.SaveChangesAsync();
        }

        public async Task Update(Pet pet)
        {
            _context.Pets.Update(pet);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Pet>> GetByCategoryId(int categoryId)
        {
            return await _context.Pets
                    .Where(x =>x.PetCategoryId == categoryId)
                    .ToListAsync();

        }
    }
}
