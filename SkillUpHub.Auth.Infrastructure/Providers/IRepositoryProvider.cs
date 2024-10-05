using SkillUpHub.Auth.Contract.Repositories;

namespace SkillUpHub.Auth.Infrastructure.Providers
{
    public interface IRepositoryProvider
    {
        public IUserRepository UserRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
    }
}