using Shopping_API.Dto.Category;
using Shopping_API.Dto.Product;
using Shopping_API.Extensions.Exceptions;
using Shopping_API.Models;
using Shopping_API.Repositories.IRepositories;
using Shopping_API.Services.IServices;

namespace Shopping_API.Services.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IRedisRepository _redisRepository;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IRedisRepository redisRepository
    )
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _redisRepository = redisRepository;
    }

    public async Task<GetCategoryResponse> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new NotFoundException("doesnt exist by this id", 404);
        }

        GetCategoryResponse categoryResponse = new(
            id: category.Id,
            name: category.Name,
            description: category.Description,
            parentCategory: category.ParentCategory is not null
                ? new GetParentCategoryResponse(
                    id: category.ParentCategory.Id,
                    name: category.ParentCategory.Name,
                    description: category.ParentCategory.Description)
                : null,
            childCategories: category.ChildCategories is not null
                ? category.ChildCategories.Select(c => new GetCategoryListResponse(
                    id: c.Id,
                    name: c.Name,
                    description: c.Description)).ToList()
                : null,
            products: category.Products is not null
                ? category.Products.Select(p => new ProductResponse(
                    id: p.Id,
                    name: p.Name,
                    description: p.Description,
                    price: p.Price,
                    stockQuantity: p.StockQuantity)).ToList()
                : null
        );
        return categoryResponse;
    }

    public async Task CreateAsync(CreateCategoryRequest createCategoryRequest)
    {
        var existCategory = await _categoryRepository.CheckExistByNameAsync(createCategoryRequest.Name);
        if (existCategory) throw new DuplicateException("category with this name is exist", 409);

        var parentCategory = await _categoryRepository.GetByIdAsync(createCategoryRequest.ParentCategoryId);

        var products = await _productRepository.GetListByIdsAsync(createCategoryRequest.ProductsId);
        var missingProductIds = createCategoryRequest.ProductsId.Except(products.Select(p => p.Id)).ToList();
        if (missingProductIds.Count != 0)
            throw new CustomException($"Some product IDs do not exist in the database{missingProductIds}", 404);

        var childCategories = _categoryRepository.GetListByIdsAsync(createCategoryRequest.ChildCategoriesId);
        var missingChildCategoryIds =
            createCategoryRequest.ChildCategoriesId.Except(childCategories.Select(c => c.Id)).ToList();
        if (missingChildCategoryIds.Count != 0)
            throw new CustomException($"Some childCategory IDs do not exist in the database{missingChildCategoryIds}",
                404);

        var category = new Category
        {
            Name = createCategoryRequest.Name,
            Description = createCategoryRequest.Description,
            ParentCategory = parentCategory is not null ? parentCategory : null,
            ChildCategories = childCategories,
            Products = products
        };
        await _categoryRepository.CreateAsync(category);
    }

    public Task<List<Category>> GetAllAsync()
    {
        var categories = _categoryRepository.GetAllAsync();
        return categories;
    }

    public async Task UpdateAsync(UpdateCategoryRequest updateCategoryRequest)
    {
        var existCategory = await _categoryRepository.GetByIdAsync(updateCategoryRequest.Id);
        if (existCategory == null) throw new DuplicateException("category with this name isn't exist", 409);

        var existParentCategory = await _categoryRepository.GetByIdAsync(updateCategoryRequest.ParentCategoryId.Value);
        if (existParentCategory == null)
            throw new DuplicateException("Parent category with this name isn't exist", 409);

        existCategory.Name = updateCategoryRequest.Name;
        existCategory.Description = updateCategoryRequest.Description;
        existCategory.ParentCategory = existParentCategory;

        await _categoryRepository.UpdateAsync(existCategory);
    }

    public async Task DeleteAsync(int id)
    {
        var existCategory = await _categoryRepository.GetByIdAsync(id);
        if (existCategory == null) throw new NotFoundException("category with this id is not exist", 404);

        await _categoryRepository.DeleteAsync(existCategory);
    }

    public async Task<List<RedisProducts>?>? getProductsByCategoryId(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null || category.Products.Count == 0)
        {
            throw new NotFoundException("doesnt exist by this id", 404);
        }

        var redisData = await _redisRepository.GetProductsByCategoryId(category.Id);
        if (redisData == null && category.Products != null)
        {
            var productsList = category.Products.Select(p => new RedisProducts
            (
                id: p.Id,
                name: p.Name,
                description: p.Description,
                price: p.Price,
                stockQuantity: p.StockQuantity
            )).ToList();
            await _redisRepository.AddCategoryProducts(category.Id, productsList);
            return productsList;
        }
        return redisData;
    }
}    