using Microsoft.EntityFrameworkCore;
using Shopping_API.Data;
using Shopping_API.Models;
using Shopping_API.Repositories.IRepositories;

namespace Shopping_API.Repositories.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dataContext;

    public UserRepository(ApplicationDbContext dataContext)
    {
        this._dataContext = dataContext;
    }

    public async Task CreateAsync(User user)
    {
        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _dataContext.Users.FirstOrDefaultAsync(user => user.userName == userName);
    }
   
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == id);
    }
    public async Task<bool> ValidateAsync(User user)
    {
        bool combinationExists = await _dataContext.Users
            .AnyAsync(x => x.userName == user.userName
                           && x.email == user.email);

        if (!combinationExists)
        {
            return false;
        }
        return true;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();
        return user;
    }
    public async Task DeleteAsync(User user)
    {
        _dataContext.Users.Remove(user);
        await _dataContext.SaveChangesAsync();
    }
}