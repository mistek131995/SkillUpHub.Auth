using SkillUpHub.Auth.Contract.Repositories;
using SkillUpHub.Command.Contract.Repositories;

namespace SkillUpHub.Command.Infrastructure.Interfaces
{
    public interface IRepositoryProvider
    {
        public IUserRepository UserRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
    }
}