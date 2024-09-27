using SkillUpHub.Auth.Data.Interfaces.Repositories;

namespace SkillUpHub.Auth.Data.Interfaces
{
    public interface IRepositoryProvider
    {
        public IUserRepository UserRepository { get; }
    }
}