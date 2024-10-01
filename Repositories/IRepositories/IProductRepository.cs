using Shopping_API.Models;

namespace Shopping_API.Repositories.IRepositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int productId);
    Task<Product> CreateAsync(Product product);
    Task<List<Product>> GetListByIdsAsync(List<int> ids);
    Task<bool> GetExistByNameAsync(string name);
    Task<int> DeleteAsync(Product product);
    Task<Product> UpdateAsync(Product product);
}