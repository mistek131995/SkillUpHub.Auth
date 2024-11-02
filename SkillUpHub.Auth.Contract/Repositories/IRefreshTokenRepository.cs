using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Data.Interfaces;
using SkillUpHub.Command.Contract.Models;

namespace SkillUpHub.Auth.Contract.Repositories;

public interface IRefreshTokenRepository : IBaseRepository
{
    public Task<RefreshToken> GetByToken(string token);
    public Task<List<RefreshToken>> GetByUserIdAsync(Guid userId);
    public Task<RefreshToken> SaveAsync(RefreshToken refreshToken);
}