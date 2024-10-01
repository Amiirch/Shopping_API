using Microsoft.AspNetCore.Mvc;
using Shopping_API.Dto.Auth;
using Shopping_API.Dto.Auth;
using Shopping_API.Extensions.Exceptions;
using Shopping_API.Services.IServices;


namespace Shopping_API.Controllers;


[Route("api/[Controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) 
    {
        _authService = authService;
    }

    [HttpPost("signUp")]
    public async Task<ActionResult> Register([FromBody] SignUpRequest signUpRequest)
    {
        try
        {
            await _authService.Register(signUpRequest);
            return Ok("Created Successfully");
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }

    }

    [HttpPost("SignIn")]
    public async Task<ActionResult> SignIn([FromBody] SignInRequest signInRequest)
    {
        try
        {
            var token = await _authService.SignIn(signInRequest);
            return Ok(token);
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }
}