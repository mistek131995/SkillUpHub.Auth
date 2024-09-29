using SkillUpHub.Auth.Domain.Repositories;

namespace SkillUpHub.Auth.Domain.Providers
{
    public interface IRepositoryProvider
    {
        public IUserRepository UserRepository { get; }
    }
}