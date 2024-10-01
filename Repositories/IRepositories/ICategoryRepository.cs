using Shopping_API.Models;

namespace Shopping_API.Repositories.IRepositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int categoryId);
    Task<Category> CreateAsync(Category category);
    Task<List<Category>> GetAllAsync();
    Task<bool> CheckExistByNameAsync(string categoryName);
    List<Category> GetListByIdsAsync(List<int> childIds);
    Task DeleteAsync(Category category);
    Task<Category> UpdateAsync(Category category);

}