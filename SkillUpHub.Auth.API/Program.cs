using SkillUpHub.Auth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AuthService>();

app.Run();