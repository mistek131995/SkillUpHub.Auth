using System;
using UUIDNext;

namespace SkillUpHub.Auth.Contract.Models;

public class User(Guid id, string login, string password, string email)
{
    public Guid Id { get; set; } = id;
    public string Login { get; set; } = login;
    public string Password { get; set; } = password;
    public string Email { get; set; } = email;

    public User(string login, string password, string email) : this(Uuid.NewDatabaseFriendly(Database.PostgreSql), login, password, email)
    {
    }
}