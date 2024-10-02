using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillUpHub.Auth.Infrastructure.Entities;

public class RefreshToken
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [MaxLength(250)]
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    [MaxLength(512)]
    public string Fingerprint { get; set; }
    [MaxLength(512)]
    public string UserAgent { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid UserId { get; set; }
    
    public User User { get; set; }
}