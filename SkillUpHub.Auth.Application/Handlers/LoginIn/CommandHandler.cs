using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Application.Handlers.LoginIn
{
    internal class CommandHandler : IRequestHandler<Command, (string accessToken, string refreshToken)>
    {
        public Task<(string accessToken, string refreshToken)> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
