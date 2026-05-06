using FluentValidation;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Validators;

public class CreateAnimalValidator : AbstractValidator<CreateAnimalRequest>
{
    public CreateAnimalValidator()
    {
        RuleFor(x => x.Identifier).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Species).IsInEnum();
        RuleFor(x => x.HealthStatus).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description is not null);
    }
}

public class UpdateAnimalValidator : AbstractValidator<UpdateAnimalRequest>
{
    public UpdateAnimalValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Species).IsInEnum();
        RuleFor(x => x.HealthStatus).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description is not null);
    }
}
