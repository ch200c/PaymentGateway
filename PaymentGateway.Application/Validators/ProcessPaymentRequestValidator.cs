using FluentValidation;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.ProcessPayment;

namespace PaymentGateway.Application.Validators;

public class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
{
    public ProcessPaymentRequestValidator(IDateTimeProvider dateTimeProvider)
    {
        RuleFor(request => request.CardNumber)
            .NotNull()
            .Length(16)
            .WithMessage($"'{nameof(ProcessPaymentRequest.CardNumber)}' has to be 16 digits");

        RuleFor(request => request.ExpiryDate)
            .SetValidator(new ExpiryDateValidator(dateTimeProvider))
            .WithMessage($"'{nameof(ProcessPaymentRequest.ExpiryDate)}' can't be in the past");

        RuleFor(request => request.Amount)
            .NotNull()
            .GreaterThan(0.0m)
            .WithMessage($"'{nameof(ProcessPaymentRequest.Amount)}' has to be positive");

        RuleFor(request => request.CurrencyCode)
            .NotNull()
            .Length(3);

        RuleFor(request => request.Cvv)
            .NotNull()
            .Length(3)
            .WithMessage($"'{nameof(ProcessPaymentRequest.Cvv)}' has to 3 digits");
    }
}
