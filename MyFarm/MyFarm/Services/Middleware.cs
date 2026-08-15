namespace MyFarm.Services;

public static class Middleware
{
    public static IApplicationBuilder AddMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        
        return app;
    }
}