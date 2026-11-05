using fruit.Data;
using fruit.Models;
using fruit.Repositories;
using Microsoft.EntityFrameworkCore;

public class FruitRepository : IFruitRepository
{
    private readonly AppDbContext _context;

    public FruitRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Fruit>> GetAll()
    {
        return await _context.Fruits.ToListAsync();
    }

    public async Task<Fruit?> GetById(int id)
    {
        return await _context.Fruits.FindAsync(id);
    }

    public async Task Add(Fruit fruit)
    {
        await _context.Fruits.AddAsync(fruit);

        await _context.SaveChangesAsync();
    }

    public async Task Update(Fruit fruit)
    {
        _context.Fruits.Update(fruit);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var fruit = await _context.Fruits.FindAsync(id);

        if (fruit != null)
        {
            _context.Fruits.Remove(fruit);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Fruit>> GetByCategoryId(int categoryId)
    {
        return await _context.Fruits
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();
    }
}