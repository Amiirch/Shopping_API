using Shopping_API.Dto.Category;
using Shopping_API.Dto.Product;
using Shopping_API.Extensions.Dto.Product;
using Shopping_API.Extensions.Exceptions;
using Shopping_API.Models;
using Shopping_API.Repositories.IRepositories;
using Shopping_API.Services.IServices;

namespace Shopping_API.Services.Services;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IRedisRepository _redisRepository;
    
    public ProductService(IProductRepository productRepository,ICategoryRepository categoryRepository,IRedisRepository redisRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _redisRepository = redisRepository;
    }

    public async Task<GetProductResponse> GetById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException("product with this id isn't exist", 404);
        }

        GetProductResponse productResponse = new(
            id: product.Id,
            name: product.Name,
            description: product.Description,
            price: product.Price,
            stockQuantity: product.StockQuantity,
            categories: product.Categories.Select(c => new GetProductCategoriesResponse(
                id: c.Id,
                name: c.Name,
                description: c.Description)).ToList()
        );
        return productResponse;
    }

    public async Task CreateAsync(CreateProductRequest createProductRequest)
    {
        var existProduct = await _productRepository.GetExistByNameAsync(createProductRequest.Name);
        if (existProduct) throw new DuplicateException("product with this name is exist", 409);

        var categories =  _categoryRepository.GetListByIdsAsync(createProductRequest.CategoryIds);
        var missedCategories = createProductRequest.CategoryIds.Except(categories.Select(c => c.Id)).ToList();
        if (missedCategories.Count != 0) throw new CustomException($"some categoriesId is not exist {missedCategories}", 404);
        
        var product = new Product();
        product.Name = createProductRequest.Name;
        product.Description = createProductRequest.Description;
        product.Price = createProductRequest.Price;
        product.StockQuantity = createProductRequest.StockQuantity;
        product.Categories = categories ;
        
        var createdProduct = await _productRepository.CreateAsync(product);
        var redisProduct = new RedisProducts(
            id: createdProduct.Id,
            name: createdProduct.Name,
            description: createdProduct.Description,
            price: createdProduct.Price,
            stockQuantity: createdProduct.StockQuantity);
        
        foreach (var category in categories)
        {
            await _redisRepository.AddProduct(category.Id, redisProduct);
        }
    }

    public async Task<Product> UpdateAsync(UpdateProductRequest updateProductRequest)
    {
        var existProduct = await _productRepository.GetByIdAsync(updateProductRequest.Id);
        if (existProduct == null) throw new NotFoundException("product with this Id isn't exist", 404);
        
        existProduct.Name = updateProductRequest.Name;
        existProduct.Description = updateProductRequest.Description;
        existProduct.Price = updateProductRequest.Price;
        existProduct.StockQuantity = updateProductRequest.StockQuantity;
        
        var updatedProduct = await _productRepository.UpdateAsync(existProduct);
        await _redisRepository.UpdateData(existProduct);
        return updatedProduct;
    }
    public async Task DeleteAsync(int id)
    {
        var existProduct = await _productRepository.GetByIdAsync(id);
        if (existProduct == null) throw new NotFoundException("product with this id is not exist", 404);

        var deletedProductId = await _productRepository.DeleteAsync(existProduct);
            
        foreach (var category in existProduct.Categories)
        {
            var redisData = await _redisRepository.GetProductsByCategoryId(category.Id);
            if (redisData != null)
            {
                var existingProduct = redisData.FirstOrDefault(p => p.Id == existProduct.Id);
                if (existingProduct != null)
                {
                    redisData.Remove(existingProduct);
                }
            }
           
            await _redisRepository.AddCategoryProducts(category.Id, redisData);
        }
    }
}