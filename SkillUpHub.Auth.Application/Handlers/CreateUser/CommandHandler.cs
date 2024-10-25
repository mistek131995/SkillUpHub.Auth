using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    internal class CommandHandler : IRequestHandler<Command, Guid>
    {
        public Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
