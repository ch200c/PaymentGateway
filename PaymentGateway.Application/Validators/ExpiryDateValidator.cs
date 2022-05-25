using FluentValidation;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain.ValueObjects;

namespace PaymentGateway.Application.Validators;

public class ExpiryDateValidator : AbstractValidator<CardExpiryDate>
{
    public ExpiryDateValidator(IDateTimeProvider dateTimeProvider)
    {
        var dateTime = dateTimeProvider.GetDateTime();

        RuleFor(expiryDate => expiryDate.Year)
            .NotNull()
            .GreaterThanOrEqualTo(dateTime.Year);

        RuleFor(expiryDate => expiryDate.Month)
            .NotNull()
            .GreaterThanOrEqualTo(dateTime.Month)
            .When(expiryDate => expiryDate.Year == dateTime.Year);
    }
}