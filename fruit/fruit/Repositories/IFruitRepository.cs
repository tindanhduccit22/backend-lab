using fruit.Models;

namespace fruit.Repositories;

public interface IFruitRepository
{
    Task<List<Fruit>> GetAll();

    Task<Fruit?> GetById(int id);

    Task Add(Fruit fruit);

    Task Update(Fruit fruit);

    Task Delete(int id);

    Task<List<Fruit>> GetByCategoryId(int categoryId);
}