using Shopping_API.Repositories.IRepositories;
using StackExchange.Redis;
using Newtonsoft.Json;
using Shopping_API.Dto.Product;
using Shopping_API.Models;


namespace Shopping_API.Repositories.Repositories;

public class RedisRepository: IRedisRepository
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisRepository(IConfiguration configuration)
    {
        _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        _db = _redis.GetDatabase();
    }
    public IDatabase GetDatabase()
    {
        return _db;
    }

    public async Task AddCategoryProducts(int categoryId, List<RedisProducts>? products)
    {
        var productsJson = JsonConvert.SerializeObject(products);
        var key = $"categoryId:{categoryId}";
        await _db.StringSetAsync(key, productsJson);
    }
    public async Task AddProduct(int categoryId, RedisProducts product)
    {
        var key = $"categoryId:{categoryId}";
        
        var existingProductsJson = await _db.StringGetAsync(key);

        var existingProducts = string.IsNullOrEmpty(existingProductsJson)
            ? null
            : JsonConvert.DeserializeObject<List<RedisProducts>>(existingProductsJson);

        if (existingProducts == null)
        {
            existingProducts = new List<RedisProducts>();
        }

        existingProducts.Add(product);
        var updatedProductsJson = JsonConvert.SerializeObject(existingProducts);
        await _db.StringSetAsync(key, updatedProductsJson);
    }
    public async Task<List<RedisProducts>?> GetProductsByCategoryId(int categoryId)
    {
        var key = $"categoryId:{categoryId}";
        var productsJson = await _db.StringGetAsync(key);
        
        if (string.IsNullOrEmpty(productsJson)) return null; 
               
        return JsonConvert.DeserializeObject<List<RedisProducts>>(productsJson);
    }
    
    public async Task UpdateData(Product existProduct)
    {
        RedisProducts updatedProduct = new
        (
            id: existProduct.Id,
            name: existProduct.Name,
            description: existProduct.Description,
            price: existProduct.Price,
            stockQuantity: existProduct.StockQuantity
        );
        
        foreach (var category in existProduct.Categories)
        {
            var redisData = await GetProductsByCategoryId(category.Id);
            if (redisData != null)
            {
                var existingProduct = redisData.FirstOrDefault(p => p.Id == existProduct.Id);
                if (existingProduct != null)
                {
                    redisData.Remove(existingProduct);
                }
            }
            else
            {
                redisData = new List<RedisProducts>();
            }
            redisData.Add(updatedProduct);
            await AddCategoryProducts(category.Id, redisData);
        }
    }
}