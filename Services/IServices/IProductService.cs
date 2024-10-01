using Shopping_API.Extensions.Dto.Product;
using Shopping_API.Models;

namespace Shopping_API.Services.IServices;

public interface IProductService
{
    Task<GetProductResponse> GetById(int productId);
    Task CreateAsync(CreateProductRequest createProductRequest);
    Task<Product> UpdateAsync(UpdateProductRequest updateUserRequest);
    Task DeleteAsync(int id);
}