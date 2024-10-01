using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping_API.Dto.User;
using Shopping_API.Extensions.Dto.User;
using Shopping_API.Extensions.Exceptions;
using Shopping_API.Services.IServices;

namespace Shopping_API.Controllers;

[Route("api/[Controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
    [HttpGet("{userId:int}")]
    public async Task<ActionResult> GetByIdAsync([FromRoute] int userId)
    {
        try
        {
            var user = await _userService.GetById(userId);
            return Ok(user);
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }

    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CreateUserRequest createUserRequest)
    {
        try
        {
            await _userService.CreateAsync(createUserRequest);
            return Ok("user created successfully");
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }
    
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody] UpdateUserRequest updateUserRequest)
    {
        try
        {
            await _userService.UpdateAsync(updateUserRequest);
            return Ok("user updated successfully");
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }
    
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
    [HttpDelete("{userId:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int userId)
    {
        try
        {
            await _userService.DeleteAsync(userId);
            return Ok("user deleted successfully");
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }
}