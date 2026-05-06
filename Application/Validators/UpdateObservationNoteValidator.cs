using FluentValidation;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Validators;

public class UpdateObservationNoteValidator : AbstractValidator<UpdateObservationNoteRequest>
{
    public UpdateObservationNoteValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(1000);
    }
}
