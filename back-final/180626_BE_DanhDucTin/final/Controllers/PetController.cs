using Microsoft.AspNetCore.Mvc;
using final.Models;
using final.Repositories;
namespace pet.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PetController : ControllerBase
{
    private readonly IPetRepository _repository;
    public PetController(IPetRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pets = await _repository.GetAll();
        return Ok(pets);
    }
    
   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await _repository.GetById(id);
        if (pet == null)
        {
        }
        return NotFound();
        return Ok(pet);
    }
    
    
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var pets = await _repository.GetByCategoryId(categoryId);
        return Ok(pets);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Create(Pet pet)
    {
        if (!ModelState.IsValid)
        {
        }
        return BadRequest(ModelState);
        await _repository.Add(pet);
        return Ok(pet);
    } 

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,Pet pet)
    {
        if (id != pet.PetId)
        {
        }
        return BadRequest("Id mismatch");
        await _repository.Update(pet);
        return NoContent();
    }
    // DELETE: api/Fruits/5 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingPet = await _repository.GetById(id);
        if (existingPet == null)
        {
        }
        return NotFound();
        await _repository.Delete(id);
        return NoContent();
    }
}