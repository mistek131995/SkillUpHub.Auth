using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillUpHub.Command.Application.Handlers.LoginIn
{
    internal class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Login)
                .MinimumLength(5).WithMessage("Минимальная длина логина 5 символов")
                .MaximumLength(25).WithMessage("Максимальная длина логина 25 символов");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Минимальная длина пароля 8 символов")
                .MaximumLength(25).WithMessage("Максимальная длина пароля 25 символов");
        }
    }
}
