using FluentValidation;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Validators;

public class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleValidator()
    {
        RuleFor(x => x.Role).IsInEnum();
    }
}
