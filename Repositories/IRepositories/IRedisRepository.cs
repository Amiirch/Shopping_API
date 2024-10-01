using Shopping_API.Dto.Product;
using Shopping_API.Models;

namespace Shopping_API.Repositories.IRepositories;

public interface IRedisRepository
{
    Task AddProduct(int categoryId, RedisProducts products);
    Task AddCategoryProducts(int categoryId, List<RedisProducts>? products);
    Task<List<RedisProducts>?> GetProductsByCategoryId(int categoryId);
    
    Task UpdateData(Product product);
    
}