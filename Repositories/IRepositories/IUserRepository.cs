using Shopping_API.Models;

namespace Shopping_API.Repositories.IRepositories;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string userName);
    Task<User?> GetByIdAsync(int id);
    Task CreateAsync(User user);
    Task<bool> ValidateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(User user);
}