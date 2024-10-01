using Moq;
using Shopping_API.Dto.Product;
using Shopping_API.Extensions.Dto.Product;
using Shopping_API.Extensions.Exceptions;
using Shopping_API.Models;
using Shopping_API.Repositories.IRepositories;
using Shopping_API.Services.Services;
using Xunit;


namespace Shopping_API.Test;

public class ProductServiceTest
{
    private readonly ProductService _productService;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IRedisRepository> _redisRepositoryMock;
    
    public ProductServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _redisRepositoryMock = new Mock<IRedisRepository>();
        _productService = new ProductService(_productRepositoryMock.Object,_categoryRepositoryMock.Object,_redisRepositoryMock.Object);
    }
    
    [Fact]
    public async Task GetById()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "High-end gaming laptop",
            Price = 1500.0M,
            StockQuantity = 10,
            Categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices" }
            }
        };

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(product);

        var result = await _productService.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal("High-end gaming laptop", result.Description);
        Assert.Equal(1500.0M, result.Price);
        Assert.Equal(10, result.StockQuantity);
        Assert.Single(result.Categories);
        Assert.Equal("Electronics", result.Categories[0].Name);
    }
    
    [Fact]
    public async Task GetByIdWhenProductNotExist()
    {
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _productService.GetById(1));

        Assert.Equal("product with this id isn't exist", exception.Message);
        Assert.Equal(404, exception.ErrorCode);
    }

    [Fact]
    public async Task CreateAsyncWhenProductExists()
    {
        var createRequest = new CreateProductRequest
        {
            Name = "ExistingProduct",
            Description = "Some Description",
            Price = 100,
            StockQuantity = 10,
            CategoryIds = new List<int> { 1, 2 }
        };
        _productRepositoryMock.Setup(repo => repo.GetExistByNameAsync(createRequest.Name))
            .ReturnsAsync(true);
        
        var exception = await Assert.ThrowsAsync<DuplicateException>(() => _productService.CreateAsync(createRequest));
        
        Assert.Equal("product with this name is exist",exception.Message);
        Assert.Equal(409,exception.ErrorCode);
    }

    [Fact]
    public async Task CreateProductWhenSomeCategoryDoesntExist()
    {
        var createRequest = new CreateProductRequest
        {
            Name = "NewProduct",
            Description = "Some Description",
            Price = 100,
            StockQuantity = 10,
            CategoryIds = new List<int> { 1, 2, 3 }
        };

        var existingCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
        };
        _categoryRepositoryMock.Setup(repo => repo.GetListByIdsAsync(createRequest.CategoryIds))
            .Returns(existingCategories);
        var exception = await Assert.ThrowsAsync<CustomException>(() => _productService.CreateAsync(createRequest));
        
        Assert.Contains("some categoriesId is not exist", exception.Message);
        Assert.Equal(404, exception.ErrorCode);
    }

    [Fact]
    public async Task CreateProductSuccessfully()
    {
        var createRequest = new CreateProductRequest
        {
            Name = "NewProduct",
            Description = "Some Description",
            Price = 100,
            StockQuantity = 10,
            CategoryIds = new List<int> { 1, 2 }
        };

        var existingCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
        };

        var createdProduct = new Product
        {
            Id = 1,
            Name = "NewProduct",
            Description = "Some Description",
            Price = 100,
            StockQuantity = 10,
            Categories = existingCategories
        };
        _productRepositoryMock.Setup(repo => repo.GetExistByNameAsync(createRequest.Name))
            .ReturnsAsync(false);

        _categoryRepositoryMock.Setup(repo => repo.GetListByIdsAsync(createRequest.CategoryIds))
            .Returns(existingCategories);

        _productRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(createdProduct);

        await _productService.CreateAsync(createRequest);
        
        _productRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Product>()), Times.Once);

        _redisRepositoryMock.Verify(redis => redis.AddProduct(1, It.IsAny<RedisProducts>()), Times.Once);
        _redisRepositoryMock.Verify(redis => redis.AddProduct(2, It.IsAny<RedisProducts>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProductWhenDoesntExist()
    {
        var updateRequest = new UpdateProductRequest()
        {
            Id = 1,
            Name = "Laptop",
            Description = "High-end gaming laptop",
            Price = 1500.0M,
            StockQuantity = 10,
        };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(updateRequest.Id))
            .ReturnsAsync((Product)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _productService.UpdateAsync(updateRequest));
        
        Assert.Equal("product with this Id isn't exist",exception.Message);
        Assert.Equal(404,exception.ErrorCode);
    }
    [Fact]
    public async Task UpdateProductSuccesssfully()
    {
        var updateRequest = new UpdateProductRequest()
        {
            Id = 1,
            Name = "Laptop",
            Description = "High-end gaming laptop",
            Price = 1500.0M,
            StockQuantity = 10,
        };
        var existProduct = new Product()
        {
            Id = 1,
            Name = "Laptop",
            Description = "High-end gaming laptop",
            Price = 2500.0M,
            StockQuantity = 20,
        };
        var updatedProduct = new Product()
        {
            Id = 1,
            Name = "Laptop",
            Description = "High-end gaming laptop",
            Price = 1500.0M,
            StockQuantity = 10,
        };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(updateRequest.Id))
            .ReturnsAsync(existProduct);

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(updateRequest.Id))
            .ReturnsAsync(updatedProduct);
        
        _redisRepositoryMock.Setup(redis => redis.UpdateData(It.IsAny<Product>()));
        
        await _productService.UpdateAsync(updateRequest);
        
        Assert.Equal(updatedProduct.Price, updateRequest.Price);
        Assert.Equal(updatedProduct.StockQuantity, updateRequest.StockQuantity);
    }
    
    [Fact]
    public async Task DeleteByIdWhenProductNotExist()
    {
        var productId = 1;

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync((Product)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _productService.GetById(1));

        Assert.Equal("product with this id isn't exist", exception.Message);
        Assert.Equal(404, exception.ErrorCode);
    }
    
    [Fact]
    public async Task DeleteByIdWhenProductExist()
    {
        var productId = 1;
        
        var product = new Product
        {
            Id = productId,
            Name = "ProductToDelete",
            Categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            }
        };

        var redisProductsCategory1 = new List<RedisProducts>
        {
            new RedisProducts ( 1, "Smartphone", "High-end smartphone with 128GB storage", 799.99m, 50 )
        };

        var redisProductsCategory2 = new List<RedisProducts>
        {
            new RedisProducts ( 2, "Smartphone", "High-end smartphone with 128GB storage", 900.99m, 100 )
        };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);
        
        _productRepositoryMock.Setup(repo => repo.DeleteAsync(product))
            .ReturnsAsync(productId);
        
        _redisRepositoryMock.Setup(redis => redis.GetProductsByCategoryId(1))
            .ReturnsAsync(redisProductsCategory1);

        _redisRepositoryMock.Setup(redis => redis.GetProductsByCategoryId(2))
            .ReturnsAsync(redisProductsCategory2);

        await _productService.DeleteAsync(productId);
        
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(product), Times.Once);

        _redisRepositoryMock.Verify(redis => redis.AddCategoryProducts(1, It.IsAny<List<RedisProducts>>()), Times.Once);
       
        _redisRepositoryMock.Verify(redis => redis.AddCategoryProducts(2, It.IsAny<List<RedisProducts>>()), Times.Once);

        Assert.DoesNotContain(redisProductsCategory1, p => p.Id == productId);
        Assert.DoesNotContain(redisProductsCategory2, p => p.Id == productId);

    }
}
    