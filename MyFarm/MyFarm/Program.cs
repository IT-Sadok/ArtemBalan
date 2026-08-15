using MyFarm.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);
var app = builder.Build();
app.AddMiddleware();
app.AddMapEndpoints();

app.Run();
