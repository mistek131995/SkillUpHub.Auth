using System;
using System.Threading.Tasks;
using SkillUpHub.Auth.Contract.Models;
using SkillUpHub.Auth.Data.Interfaces;

namespace SkillUpHub.Auth.Contract.Repositories;

public interface IRefreshTokenRepository : IBaseRepository
{
    public Task<RefreshToken> GetByToken(string token);
    public Task<RefreshToken> GetByUserIdAsync(Guid userId);
    public Task<RefreshToken> SaveAsync(RefreshToken refreshToken);
}