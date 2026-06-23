using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyFarm.Data;
using MyFarm.Models;

namespace MyFarm.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(UserModel model);
    Task<string?> LoginAsync(UserModel model);
}

public class AuthService : IAuthService
{
    private UserManager<IdentityUser> _userManager;

    public AuthService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterAsync(UserModel model)
    {
        var user = new IdentityUser { UserName = model.Username, Email = model.Username };
        return await _userManager.CreateAsync(user, model.Password);
    }

    public async Task<string?> LoginAsync(UserModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(50)),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        return null;
    }
}