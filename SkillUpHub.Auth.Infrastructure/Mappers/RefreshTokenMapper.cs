using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using CRefreshToken = SkillUpHub.Auth.Contract.Models.RefreshToken;
using IRefreshToken = SkillUpHub.Auth.Infrastructure.Entities.RefreshToken;

namespace SkillUpHub.Auth.Infrastructure.Mappers;

[Mapper]
public partial class RefreshTokenMapper
{
    public partial CRefreshToken MappingToContractModel(IRefreshToken source);
    public partial List<CRefreshToken> MappingToContractModels(List<IRefreshToken> sources);
    public partial IRefreshToken MappingToInfrastructureModel(CRefreshToken source);
}