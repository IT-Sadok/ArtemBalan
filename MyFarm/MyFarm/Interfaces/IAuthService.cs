using MyFarm.Models;
using Microsoft.AspNetCore.Identity;

namespace MyFarm.Interfaces;
public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterModel model,CancellationToken cancellationToken);
    Task<LoginResponse> LoginAsync(LoginModel model,CancellationToken cancellationToken);
}
