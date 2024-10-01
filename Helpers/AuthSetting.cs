using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shopping_API.Models;

namespace Shopping_API.Extensions.Helpers;

public class AuthSetting
{ 
    public string PrivateKey { get; set; }
}