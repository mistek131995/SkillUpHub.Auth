using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Application.Handlers.LoginIn
{
    public class Command : IRequest<(string accessToken, string refreshToken)>
    {
        public string Login {  get; set; }
        public string Password { get; set; }
        public string UserAgent { get; set; }
        public string FingerPrint { get; set; }
        public string? RefreshToken { get; set; }
    }
}
