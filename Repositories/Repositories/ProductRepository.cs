using Microsoft.EntityFrameworkCore;
using Shopping_API.Data;
using Shopping_API.Models;
using Shopping_API.Repositories.IRepositories;

namespace Shopping_API.Repositories.Repositories;

public class ProductRepository:IProductRepository
{
    private readonly ApplicationDbContext _dataContext;

    public ProductRepository(ApplicationDbContext dataContext)
    {
        this._dataContext = dataContext;
    }
    public Task<Product?> GetByIdAsync(int productId)
    {
        return _dataContext.Products.Include(p => p.Categories)
            .FirstOrDefaultAsync(product => product.Id == productId);
    }

    public async Task<Product> CreateAsync(Product product)
    { 
        await _dataContext.Products.AddAsync(product);
        await _dataContext.SaveChangesAsync(); 
        return product;
    }
    public async Task<List<Product>> GetListByIdsAsync(List<int> ids)
    {
        var products = await  _dataContext.Products.Where(product => ids.Contains(product.Id)).ToListAsync();
        return products;
    }
    public async Task<bool> GetExistByNameAsync(string name)
    {
        return await _dataContext.Products.AnyAsync(product => product.Name == name);
    }
    public async Task<int> DeleteAsync(Product product)
    {
        _dataContext.Products.Remove(product);
        await _dataContext.SaveChangesAsync();
        return product.Id;
    }
    public async Task<Product> UpdateAsync(Product product)
    {
        _dataContext.Products.Update(product);
        await _dataContext.SaveChangesAsync();
        return product;
    }
}