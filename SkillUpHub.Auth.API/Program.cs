using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Extensions;
using SkillUpHub.Auth.Infrastructure.Clients;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Interfaces;
using SkillUpHub.Command.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommands(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddScoped<IMessageBusClient, RabbitMqClient>(x =>
    new RabbitMqClient(builder.Configuration.GetSection("RabbitMqHost").Value));

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
}

app.UseCors("AllowAll");
app.RegisterRoutes();

app.Run();