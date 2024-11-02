using MediatR;

namespace SkillUpHub.Command.Application.Handlers.RefreshToken;

public class Command : IRequest<string>
{
    public string FingerPrint { get; set; }
    public string UserAgent { get; set; }
}