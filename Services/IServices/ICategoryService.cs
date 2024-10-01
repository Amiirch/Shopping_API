using Shopping_API.Dto.Category;
using Shopping_API.Dto.Product;
using Shopping_API.Models;

namespace Shopping_API.Services.IServices;

public interface ICategoryService
{ 
    Task<GetCategoryResponse> GetByIdAsync(int id);
    Task CreateAsync(CreateCategoryRequest createCategoryRequest);
    Task<List<Category>> GetAllAsync();
    Task UpdateAsync(UpdateCategoryRequest updateUserRequest);
    Task DeleteAsync(int id);
    Task<List<RedisProducts>?>? getProductsByCategoryId(int categoryId);
    
}