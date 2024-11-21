using MediatR;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    public class Command : IRequest<Unit>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        
        public string CaptchaToken { get; set; }
        
        public Guid SessionId { get; set; }
    }
}
