using System;
using System.Security.Cryptography;
using UUIDNext;

namespace SkillUpHub.Command.Contract.Models;

public class RefreshToken(
    Guid id,
    string token,
    DateTime expiryDate,
    string fingerprint,
    string userAgent,
    DateTime createDate, 
    Guid userId)
{
    public Guid Id { get; } = id;
    public string Token { get; set; } = token;
    public DateTime ExpiryDate { get; private set; } = expiryDate;
    public bool IsRevoked { get; private set; }
    public string Fingerprint { get; private set; } = fingerprint;
    public string UserAgent { get; private set; } = userAgent;
    public DateTime CreateDate { get; private set; } = createDate;
    
    public Guid UserId { get; private set; } = userId;
    

    public RefreshToken(string token, string fingerprint, string userAgent, Guid userId) : this(Uuid.NewDatabaseFriendly(Database.PostgreSql), token, DateTime.UtcNow.AddDays(15), fingerprint, userAgent, DateTime.UtcNow, userId)
    {
    }

    public void Update(string token)
    {
        Token = token;
        ExpiryDate = DateTime.UtcNow.AddDays(15);
        CreateDate = DateTime.UtcNow;
    }

    public bool TokenIsValid(string token, string fingerprint, string userAgent)
    {
        return Token == token && Fingerprint == fingerprint && UserAgent == userAgent && ExpiryDate < DateTime.UtcNow && !IsRevoked;
    }
    
    public static string GenerateRefreshToken(int length = 32)
    {
        var randomNumber = new byte[length];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}