namespace CreditApproval.Validators;

public class ReviewCreditValidator : AbstractValidator<ReviewCreditDTO>
{
    public ReviewCreditValidator(ICreditService creditService, IUserService userService)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("You need to provide a valid id")
            .Must(creditService.Exists).WithMessage("Invalid credit identifier")
            .Must(id => !creditService.IsReviewed(id)).WithMessage("Credit was already reviewed");

        RuleFor(x => x.ReviewerName)
            .NotEmpty().WithMessage("Reviewer name must be valid")
            .Must(x => userService.Exists(x)).WithMessage("Reviewer does not exist")
            .Must(x => userService.IsAdmin(x)).WithMessage("Reviewer is not an admin");

        RuleFor(x => x.Decision)
            .NotEmpty().WithMessage("Decision is required")
            .Must(d => Enum.TryParse<CreditDecision>(d, true, out _))
            .WithMessage("Invalid credit decision")
            .DependentRules(() =>
            {
                RuleFor(x => x).Custom((dto, context) =>
                {
                    Enum.TryParse(dto.Decision, ignoreCase: true, out CreditDecision decision);

                    if (!creditService.CheckIfCanBeApproved(dto.Identifier, decision))
                    {
                        context.AddFailure(nameof(dto.Decision),
                            "This credit request cannot be approved due to exceeding the approval limit.");
                    }
                });
            });
    }
}
