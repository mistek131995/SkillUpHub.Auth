using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Data.Entities;

namespace SkillUpHub.Auth.Data.Contexts;

public class PGContext(DbContextOptions<PGContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}