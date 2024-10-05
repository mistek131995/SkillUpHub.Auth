using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Contract.Clients;
using SkillUpHub.Auth.Infrastructure.Clients;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Providers;
using SkillUpHub.Auth.Middlewares;
using SkillUpHub.Auth.Services;
using IServiceProvider = SkillUpHub.Auth.Application.Providers.IServiceProvider;
using ServiceProvider = SkillUpHub.Auth.Application.Providers.ServiceProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>(); // Регистрируем Interceptor
});

builder.Services.AddDbContext<PGContext>(option => 
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IServiceProvider, ServiceProvider>();
builder.Services.AddScoped<IRepositoryProvider, RepositoryProvider>();
builder.Services.AddScoped<IMessageBusClient, RabbitMqClient>();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
        .AllowCredentials();
}));


var app = builder.Build();

// Автоматическое применение миграций при старте приложения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PGContext>();
    dbContext.Database.Migrate();
}

app.UseGrpcWeb();

app.UseCors();

app.MapGrpcService<AuthService>().EnableGrpcWeb().RequireCors("AllowAll");

app.Run();