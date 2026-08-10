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

                if (result.Succeeded)
                    return Results.Ok("User created successfully!");
                else
                    return Results.BadRequest(result.Errors);
            });

        app.MapPost("/login", async (LoginModel model, IAuthService authService, CancellationToken cancellationToken) =>
        {
            var token = await authService.LoginAsync(model, cancellationToken);
            if (token != null)
                return Results.Ok(token);
            else
                return Results.Unauthorized();
        });

        app.MapGet("/protected", [Authorize]() =>
            "This is a secret zone for authorized users only!");

        return app;
    }
}