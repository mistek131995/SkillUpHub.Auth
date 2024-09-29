using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillUpHub.Auth.Infrastructure.Entities;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Login { get; set; }
    [MaxLength(50)]
    public string Email { get; set; }
    [MaxLength(250)]
    public string Password { get; set; }
}