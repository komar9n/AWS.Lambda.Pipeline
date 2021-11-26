using FluentValidation;

namespace TestFunctions.Queries.GetComponents
{
    public class GetComponentsValidator : AbstractValidator<GetComponentsQuery>
    {
        public GetComponentsValidator()
        {
            RuleFor(i => i.Ids).NotNull().NotEmpty();
        }
    }
}
