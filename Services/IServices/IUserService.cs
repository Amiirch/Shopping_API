using Shopping_API.Dto.User;
using Shopping_API.Extensions.Dto.User;

namespace Shopping_API.Services.IServices;

public interface IUserService
{
    Task<GetUserResponse> GetById(int id);
    Task CreateAsync(CreateUserRequest createUserRequest);
    Task UpdateAsync(UpdateUserRequest updateUserRequest);
    Task DeleteAsync(int id);
}   