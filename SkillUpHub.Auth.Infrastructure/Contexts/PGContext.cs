using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Infrastructure.Entities;

namespace SkillUpHub.Auth.Infrastructure.Contexts;

public class PGContext(DbContextOptions<PGContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}