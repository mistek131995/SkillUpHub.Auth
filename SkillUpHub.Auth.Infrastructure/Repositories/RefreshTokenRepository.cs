using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Contract.Repositories;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Mapping;

namespace SkillUpHub.Auth.Infrastructure.Repositories;

public class RefreshTokenRepository(PGContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken> GetByToken(string token)
    {
        var mapping = new RefreshTokenMapper();
        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        
        return mapping.MappingToContractModel(refreshToken);
    }

    public async Task<RefreshToken> GetByUserIdAsync(Guid userId)
    {
        var mapping = new RefreshTokenMapper();
        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
        
        return mapping.MappingToContractModel(refreshToken);
    }

    public async Task<RefreshToken> SaveAsync(RefreshToken refreshToken)
    {
        var mapping = new RefreshTokenMapper();

        var dbRefreshToken = mapping.MappingToInfrastructureModel(refreshToken);
        context.RefreshTokens.Add(dbRefreshToken);
        await context.SaveChangesAsync();

        return refreshToken;
    }
}