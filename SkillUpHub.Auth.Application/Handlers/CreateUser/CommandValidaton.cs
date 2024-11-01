using FluentValidation;

namespace SkillUpHub.Command.Application.Handlers.CreateUser
{
    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(x => x.Login)
                .MinimumLength(5).WithMessage("Минимальная длина логина 5 символов")
                .MaximumLength(25).WithMessage("Максимальная длина логина 25 символов");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Минимальная длина пароля 8 символов")
                .MaximumLength(25).WithMessage("Максимальная длина пароля 25 символов");

            RuleFor(x => x.Email)
                .MinimumLength(5).WithMessage("Минимальная длина электронной почты 5 символов")
                .MaximumLength(25).WithMessage("Максимальная длина электронной почты 25 символов");

            RuleFor(x => x.CaptchaToken)
                .NotEmpty().WithMessage("Ошибка проверки каптчи");
        }
    }
}
