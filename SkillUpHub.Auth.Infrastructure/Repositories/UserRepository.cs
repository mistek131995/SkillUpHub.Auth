﻿using SkillUpHub.Auth.Contract.Repositories;
using SkillUpHub.Auth.Infrastructure.Contexts;

namespace SkillUpHub.Auth.Infrastructure.Repositories
{
    public class UserRepository(PGContext context) : IUserRepository
    {
    }
}