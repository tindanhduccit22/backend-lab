using final.Models;
namespace final.Repositories;
public interface IPetRepository
{
    Task<List<Pet>> GetAll();
    Task<Pet?> GetById(int id);
    Task Add(Pet pet);
    Task Update(Pet pet);
    Task Delete(int id);

    Task<List<Pet>> GetByCategoryId(int categoryId);
}