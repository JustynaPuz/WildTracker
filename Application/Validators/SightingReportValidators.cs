using FluentValidation;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Validators;

public class CreateSightingReportValidator : AbstractValidator<CreateSightingReportRequest>
{
    public CreateSightingReportValidator()
    {
        RuleFor(x => x.AnimalId).NotEmpty();
        // Delegate so DateTime.UtcNow is evaluated at validation time, not at startup
        RuleFor(x => x.ObservedAtUtc).NotEmpty().LessThanOrEqualTo(_ => DateTime.UtcNow);
        RuleFor(x => x.ReportType).IsInEnum();
        RuleFor(x => x.Source).IsInEnum();
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.Region).MaximumLength(100).When(x => x.Region is not null);
        RuleFor(x => x.ForestDistrict).MaximumLength(100).When(x => x.ForestDistrict is not null);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
    }
}

public class UpdateSightingReportValidator : AbstractValidator<UpdateSightingReportRequest>
{
    public UpdateSightingReportValidator()
    {
        RuleFor(x => x.ObservedAtUtc).NotEmpty().LessThanOrEqualTo(_ => DateTime.UtcNow);
        RuleFor(x => x.ReportType).IsInEnum();
        RuleFor(x => x.Source).IsInEnum();
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.Region).MaximumLength(100).When(x => x.Region is not null);
        RuleFor(x => x.ForestDistrict).MaximumLength(100).When(x => x.ForestDistrict is not null);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
    }
}
