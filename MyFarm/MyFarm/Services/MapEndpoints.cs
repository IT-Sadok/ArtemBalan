using Microsoft.AspNetCore.Authorization;
using MyFarm.Models;

namespace MyFarm.Services;

public static class MapEndpoints
{
    public static IApplicationBuilder AddMapEndpoints(this WebApplication app)
    {
        app.MapPost("/register", async (UserModel model, IAuthService authService) =>
        {
            var result = await authService.RegisterAsync(model);

            if (result.Succeeded)
               return Results.Ok("User created successfully!");
            else
               return Results.BadRequest(result.Errors);
        });

        app.MapPost("/login", async (UserModel model, IAuthService authService) =>
        {
            var token = await authService.LoginAsync(model);
            if (token != null)
               return Results.Ok(token);
            else
               return Results.Unauthorized();
        });

        app.MapGet("/protected", [Authorize] () => 
            "This is a secret zone for authorized users only!");
        
        return app;
    }
}