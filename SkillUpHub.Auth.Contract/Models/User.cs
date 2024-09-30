using System;

namespace SkillUpHub.Auth.Contract.Models;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    public User(Guid id, string login, string password, string email)
    {
        Id = id;
        Login = login;
        Password = password;
        Email = email;
    }
    
    public User(string login, string password, string email)
    {
        Id = Guid.NewGuid();
        Login = login;
        Password = password;
        Email = email;
    }
}