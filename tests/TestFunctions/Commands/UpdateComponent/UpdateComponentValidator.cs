using FluentValidation;

namespace TestFunctions.Commands.UpdateComponent
{
    public class UpdateComponentValidator : AbstractValidator<UpdateComponentCommand>
    {
        public UpdateComponentValidator()
        {
            RuleFor(i => i.Id).GreaterThan(0);
            RuleFor(i => i.Name).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(i => i.Description).MaximumLength(100);
        }
    }
}
