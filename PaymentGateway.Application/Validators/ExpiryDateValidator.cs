using FluentValidation;
using PaymentGateway.Domain;

namespace PaymentGateway.Application.Validators;

public class ExpiryDateValidator : AbstractValidator<CardExpiryDate>
{
    public ExpiryDateValidator()
    {
        RuleFor(expiryDate => expiryDate.Year)
            .NotNull()
            .GreaterThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(expiryDate => expiryDate.Month)
            .NotNull()
            .GreaterThanOrEqualTo(DateTime.UtcNow.Month)
            .When(expiryDate => expiryDate.Year == DateTime.UtcNow.Year);
    }
}