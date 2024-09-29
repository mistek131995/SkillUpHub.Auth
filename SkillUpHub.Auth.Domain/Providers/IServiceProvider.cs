﻿using SkillUpHub.Auth.Domain.Providers;
using SkillUpHub.Auth.Domain.Services;

namespace SkillUpHub.Auth.Domain.Providers;

public interface IServiceProvider
{
    IAuthService AuthService { get; }
}