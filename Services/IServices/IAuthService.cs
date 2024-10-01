using Shopping_API.Dto.Auth;
using Shopping_API.Dto.Auth;

namespace Shopping_API.Services.IServices;

public interface IAuthService
{
    Task Register(SignUpRequest signUpRequest);

    Task<string> SignIn(SignInRequest signInRequest);
}