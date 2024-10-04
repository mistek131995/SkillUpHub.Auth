using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using CUserModel = SkillUpHub.Auth.Contract.Models.User;
using IUserModel = SkillUpHub.Auth.Infrastructure.Entities.User;

namespace SkillUpHub.Auth.Infrastructure.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial CUserModel MappingToContractModel(IUserModel user);
    public partial List<CUserModel> MappingToContractModel(List<IUserModel> users);
    public partial IUserModel MappingToInfrastructureModel(CUserModel user);
    public partial List<IUserModel> MappingToInfrastructureModel(List<CUserModel> user);
}