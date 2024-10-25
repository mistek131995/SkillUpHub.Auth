using FluentValidation;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    internal class CommandValidaton : AbstractValidator<Command>
    {
        public CommandValidaton()
        {
            RuleFor(x => x.Login)
                .MinimumLength(5).WithMessage("Минимальная длина логина 5 символов")
                .MaximumLength(20).WithMessage("Максимальная длина логина 20 символов");

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Минимальная длина пароля 6 символов")
                .MaximumLength(25).WithMessage("Максимальная длина пароля 25 символов");

            RuleFor(x => x.Email)
                .MinimumLength(5).WithMessage("Минимальная длина электронной почты 5 символов")
                .MaximumLength(25).WithMessage("Максимальная длина электронной почты 25 символов");

            RuleFor(x => x.CaptchaToken)
                .NotEmpty().WithMessage("Ошибка проверки каптчи");
        }
    }
}
