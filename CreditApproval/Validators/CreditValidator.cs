namespace CreditApproval.Validators;

public class CreditValidator : AbstractValidator<SubmitCreditDTO>
{
    private const decimal MORTGAGE_LIMIT = 500_000M;
    private const decimal AUTO_LIMIT = 50_000M;
    private const decimal PERSONAL_LIMIT = 10_000M;

    public CreditValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("Invalid name");

        RuleFor(x => x.CreditAmount)
            .GreaterThan(0)
            .WithMessage("You need to enter a valid credit amount");

        RuleFor(x => x.MonthlyIncome)
            .GreaterThan(0)
            .WithMessage("You need to enter a valid monthly income");

        RuleFor(x => x.Type)
            .Must(type => Enum.TryParse<CreditType>(type, ignoreCase: true, out _))
            .WithMessage("Invalid credit type");

        RuleFor(x => x)
            .Custom((dto, context) =>
            {
                if (!Enum.TryParse(dto.Type, ignoreCase: true, out CreditType creditType))
                {
                    return;
                }

                decimal limit = creditType switch
                {
                    CreditType.Mortgage => MORTGAGE_LIMIT,
                    CreditType.Auto => AUTO_LIMIT,
                    CreditType.Personal => PERSONAL_LIMIT,
                    _ => decimal.MaxValue
                };

                if (dto.CreditAmount > limit)
                {
                    context.AddFailure(nameof(dto.CreditAmount),
                        $"The maximum amount for {creditType} credit is {limit}");
                }
            });
    }
}
