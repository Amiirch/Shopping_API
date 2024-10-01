using Shopping_API.Dto.User;
using Shopping_API.Extensions.Dto.User;
using Shopping_API.Extensions.Exceptions;
using Shopping_API.Models;
using Shopping_API.Repositories.IRepositories;
using Shopping_API.Services.IServices;

namespace Shopping_API.Services.Services;

public class UserService:IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserResponse> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException("User with this id isn't exist", 404);
        }

        GetUserResponse userResponse = new(
            id: user.Id,
            fullName: user.fullName,
            userName: user.userName,
            roles: user.roles,
            email: user.email,
            address: user.address,
            phoneNumber: user.phoneNumber
        );
        return userResponse;
    }

    public async Task CreateAsync(CreateUserRequest createUserRequest)
    {
        var user = new User();
        user.fullName = createUserRequest.FullName;
        user.userName = createUserRequest.UserName;
        user.password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);
        user.roles = createUserRequest.Roles;
        user.email = createUserRequest.Email;
        user.address = createUserRequest.Address;
        user.phoneNumber = createUserRequest.PhoneNumber;
        
        await _userRepository.CreateAsync(user);
    }
    public async Task UpdateAsync(UpdateUserRequest updateUserRequest)
    {
        var existUser = await _userRepository.GetByIdAsync(updateUserRequest.Id);
        if (existUser == null) throw new NotFoundException("user is not exist with this id", 404);
        
        existUser.userName = updateUserRequest.UserName;
        existUser.fullName = updateUserRequest.FullName;
        existUser.address = updateUserRequest.Address;
        existUser.phoneNumber = updateUserRequest.PhoneNumber;
        existUser.email = updateUserRequest.Email;
        
        await _userRepository.UpdateAsync(existUser);
        
    }

    public async Task DeleteAsync(int id)
    {
        var existUser = await _userRepository.GetByIdAsync(id);
        if (existUser == null) throw new NotFoundException("user with this id is not exist", 404);

        await _userRepository.DeleteAsync(existUser);
    }
}