using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Application.Handlers.LoginIn
{
    internal class Command : IRequest<(string accessToken, string refreshToken)>
    {
    }
}
