using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Repositories;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Command.Infrastructure.Repositories;
using EUser = SkillUpHub.Auth.Infrastructure.Entities.User;

public class UserRepositoryTests
{
    private readonly PGContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<PGContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new PGContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new User(userId, "testlogin", "testpassword", "test@example.com");
        _context.Users.Add(new EUser { Id = userId, Login = user.Login, Password = user.Password, Email = user.Email });
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("testlogin", result.Login);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
    {
        var email = "test@example.com";
        var user = new EUser { Id = Guid.NewGuid(), Login = "testlogin", Password = "testpassword", Email = email };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByEmailAsync(email);

        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task SaveAsync_ShouldAddUser_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var user = new User(userId, "newlogin", "newpassword", "new@example.com");

        var result = await _repository.SaveAsync(user);
        var savedUser = await _context.Users.FindAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, savedUser.Id);
        Assert.Equal("newlogin", savedUser.Login);
    }

    [Fact]
    public async Task SaveAsync_ShouldUpdateUser_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new EUser { Id = userId, Login = "testlogin", Password = "testpassword", Email = "test@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var updatedUser = new User(userId, "updatedlogin", "updatedpassword", "updated@example.com");

        var result = await _repository.SaveAsync(updatedUser);
        var savedUser = await _context.Users.FindAsync(userId);

        Assert.NotNull(result);
        Assert.Equal("updatedlogin", savedUser.Login);
        Assert.Equal("updated@example.com", savedUser.Email);
    }
}
