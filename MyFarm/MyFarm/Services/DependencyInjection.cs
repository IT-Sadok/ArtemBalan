using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFarm.Data;
using MyFarm.Interfaces;

namespace MyFarm.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<AuthOptions>(
            configuration.GetSection("Jwt"));
        
        AuthOptions jwtOptions = configuration
            .GetSection("Jwt")
            .Get<AuthOptions>()!;
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("MyFarmDb"));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
        
        services.AddAuthorization();
        
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}