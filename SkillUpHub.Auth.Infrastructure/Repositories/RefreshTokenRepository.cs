using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Contract.Repositories;
using SkillUpHub.Auth.Infrastructure.Contexts;
using SkillUpHub.Auth.Infrastructure.Mappers;

namespace SkillUpHub.Auth.Infrastructure.Repositories;

public class RefreshTokenRepository(PGContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken> GetByToken(string token)
    {
        var mapping = new RefreshTokenMapper();
        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        
        return refreshToken == null ? null : mapping.MappingToContractModel(refreshToken);
    }

    public async Task<List<RefreshToken>> GetByUserIdAsync(Guid userId)
    {
        var mapping = new RefreshTokenMapper();
        var refreshTokens = await context.RefreshTokens
            .Where(t => t.UserId == userId)
            .ToListAsync();
        
        return mapping.MappingToContractModels(refreshTokens);
    }

    public async Task<RefreshToken> SaveAsync(RefreshToken refreshToken)
    {
        var mapping = new RefreshTokenMapper();
        var dbRefreshToken = mapping.MappingToInfrastructureModel(refreshToken);
        
        var existingRefreshToken = await context.RefreshTokens.FindAsync(dbRefreshToken.Id);
        
        if (existingRefreshToken != null)
            context.Entry(existingRefreshToken).CurrentValues.SetValues(dbRefreshToken);
        else
            context.RefreshTokens.Add(dbRefreshToken);
        
        await context.SaveChangesAsync();

        return refreshToken;
    }
}