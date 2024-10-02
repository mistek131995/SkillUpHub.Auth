using System;

namespace SkillUpHub.Auth.Contract.Models;

public class RefreshToken(
    Guid id,
    string token,
    DateTime expiryDate,
    string fingerprint,
    string userAgent,
    DateTime createDate)
{
    public Guid Id { get; set; } = id;
    public string Token { get; set; } = token;
    public DateTime ExpiryDate { get; set; } = expiryDate;
    public bool IsRevoked { get; set; }
    public string Fingerprint { get; set; } = fingerprint;
    public string UserAgent { get; set; } = userAgent;
    public DateTime CreateDate { get; set; } = createDate;

    public RefreshToken(string token, string fingerprint, string userAgent) : this(Guid.NewGuid(), token, DateTime.Now.AddDays(15), fingerprint, userAgent, DateTime.Now)
    {
    }
}