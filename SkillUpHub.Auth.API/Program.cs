using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Extensions;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Interfaces;
using SkillUpHub.Auth.Middlewares;
using SkillUpHub.Command.Application;
using SkillUpHub.Command.Infrastructure.Clients;
using SkillUpHub.Command.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommands(builder.Configuration);

builder.Services.AddScoped<IMessageBusClient, RabbitMqClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName); // Используем полное имя типа
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Автоматическое применение миграций при старте приложения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PGContext>();
    dbContext.Database.Migrate();
    
    var rabbitMqService = scope.ServiceProvider.GetRequiredService<IMessageBusClient>();
    rabbitMqService!.Initialize();
}

app.UseCors("AllowAll");
app.RegisterRoutes();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();