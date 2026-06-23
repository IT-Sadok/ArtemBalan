using MyFarm.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAppServices();
var app = builder.Build();
app.AddMiddleware();
app.AddMapEndpoints();

app.Run();
