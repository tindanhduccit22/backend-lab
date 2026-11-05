using Microsoft.AspNetCore.Mvc;
using fruit.Models;
using fruit.Repositories;

namespace fruit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FruitsController : ControllerBase
{
    private readonly IFruitRepository _repository;

    public FruitsController(IFruitRepository repository)
    {
        _repository = repository;
    }

    // GET: api/Fruits
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fruits = await _repository.GetAll();

        return Ok(fruits);
    }

    // GET: api/Fruits/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var fruit = await _repository.GetById(id);

        if (fruit == null)
        {
            return NotFound();
        }

        return Ok(fruit);
    }

    // GET: api/Fruits/category/1
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var fruits = await _repository.GetByCategoryId(categoryId);

        return Ok(fruits);
    }

    // POST: api/Fruits
    [HttpPost]
    public async Task<IActionResult> Create(Fruit fruit)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _repository.Add(fruit);

        return Ok(fruit);
    }

    // PUT: api/Fruits/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Fruit fruit)
    {
        if (id != fruit.Id)
        {
            return BadRequest("Id mismatch");
        }

        await _repository.Update(fruit);

        return NoContent();
    }

    // DELETE: api/Fruits/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingFruit = await _repository.GetById(id);

        if (existingFruit == null)
        {
            return NotFound();
        }

        await _repository.Delete(id);

        return NoContent();
    }
}