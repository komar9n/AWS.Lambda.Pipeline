using FluentValidation;

namespace TestFunctions.Commands.CreateComponent
{
    public class CreateComponentValidator : AbstractValidator<CreateComponentCommand>
    {
        public CreateComponentValidator()
        {
            RuleFor(i => i.Name).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(i => i.Description).MaximumLength(100);
        }
    }
}
