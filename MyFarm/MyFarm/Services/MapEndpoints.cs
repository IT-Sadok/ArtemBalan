using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFarm.Interfaces;
using MyFarm.Models;

namespace MyFarm.Services;

public static class MapEndpoints
{
    public static IApplicationBuilder AddMapEndpoints(this WebApplication app)
    {
        app.MapPost("/register",
            async (RegisterModel model, IAuthService authService, CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterAsync(model, cancellationToken);


                return result.Succeeded ? Results.Ok("User created successfully!") : Results.BadRequest(result.Errors);
            });

        app.MapPost("/login", async (LoginModel model, IAuthService authService, CancellationToken cancellationToken) =>
        {
            var token = await authService.LoginAsync(model, cancellationToken);

            return token != null ? Results.Ok(token) : Results.Unauthorized();
        });

        app.MapGet("/protected", [Authorize]() =>
            "This is a secret zone for authorized users only!");

        return app;
    }
}