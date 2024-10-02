using System;

namespace SkillUpHub.Auth.Contract.Models;

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
    public string Token { get; private set; } = token;
    public DateTime ExpiryDate { get; private set; } = expiryDate;
    public bool IsRevoked { get; private set; }
    public string Fingerprint { get; private set; } = fingerprint;
    public string UserAgent { get; private set; } = userAgent;
    public DateTime CreateDate { get; private set; } = createDate;
    
    public Guid UserId { get; private set; } = userId;
    

    public RefreshToken(string token, string fingerprint, string userAgent, Guid userId) : this(Guid.NewGuid(), token, DateTime.UtcNow.AddDays(15), fingerprint, userAgent, DateTime.UtcNow, userId)
    {
    }

    public void Update(string token)
    {
        Token = token;
        ExpiryDate = DateTime.UtcNow.AddDays(15);
        CreateDate = DateTime.UtcNow;
    }
}