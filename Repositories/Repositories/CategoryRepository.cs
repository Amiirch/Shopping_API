using Microsoft.EntityFrameworkCore;
using Shopping_API.Data;
using Shopping_API.Models;
using Shopping_API.Repositories.IRepositories;

namespace Shopping_API.Repositories.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dataContext;

    public CategoryRepository(ApplicationDbContext dataContext)
    {
        this._dataContext = dataContext;
    }
    public async Task<Category?> GetByIdAsync(int categoryId)
    {
        return await _dataContext.Categories
            .Include(c =>c.Products)
            .Include(c => c.ParentCategory)
            .Include(c => c.ChildCategories)
            .FirstOrDefaultAsync(category => category.Id == categoryId);
    }
    public async Task<bool> CheckExistByNameAsync(string categoryName)
    {
        return await _dataContext.Categories.AnyAsync(category => category.Name == categoryName);
    }
    public async Task<Category> CreateAsync(Category category)
    { 
        await _dataContext.Categories.AddAsync(category);
        await _dataContext.SaveChangesAsync(); 
        return category;
    }
    public  Task<List<Category>> GetAllAsync()
    {
        return _dataContext.Categories.ToListAsync();
    }
    public  List<Category> GetListByIdsAsync(List<int> childIds)
    {
        var categories =  _dataContext.Categories.Where(category => childIds.Contains(category.Id)).ToList();
        return categories;
    }
    public async Task DeleteAsync(Category category)
    {
        _dataContext.Categories.Remove(category);
        await _dataContext.SaveChangesAsync();
    }
    public async Task<Category> UpdateAsync(Category category)
    {
        _dataContext.Categories.Update(category);
        await _dataContext.SaveChangesAsync();
        return category;
    }
}