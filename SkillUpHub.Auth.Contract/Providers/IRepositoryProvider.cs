using SkillUpHub.Auth.Contract.Repositories;

namespace SkillUpHub.Auth.Contract.Providers
{
    public interface IRepositoryProvider
    {
        public IUserRepository UserRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
    }
}