using MediatR;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    public class Command : IRequest<Guid>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CaptchaToken { get; set; }
    }
}
