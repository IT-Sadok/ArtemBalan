using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFarm.Data;
using MyFarm.Models;
using MyFarm.Interfaces;

namespace MyFarm.Services;

public class AuthService : IAuthService
{
    private UserManager<IdentityUser> _userManager;
    private AuthOptions _jwtOptions;

    public AuthService(UserManager<IdentityUser> userManager, IOptions<AuthOptions> jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default)
    {
        var user = new IdentityUser { UserName = model.Username, Email = model.Username };
        return await _userManager.CreateAsync(user, model.Password);
    }

    public async Task<LoginResponse?> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
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
                issuer: _jwtOptions.ISSUER,
                audience: _jwtOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(50)),
                signingCredentials: new SigningCredentials(
                    _jwtOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var tokenString =new JwtSecurityTokenHandler().WriteToken(jwt);
            return new LoginResponse { Token = tokenString };
        }

        return null;
    }
}