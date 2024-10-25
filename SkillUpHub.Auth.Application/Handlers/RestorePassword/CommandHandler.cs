using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Application.Handlers.RestorePassword
{
    internal class CommandHandler : IRequestHandler<Command, Unit>
    {
        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
