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

        RuleFor(request => request.CardExpiryDate)
            .SetValidator(new ExpiryDateValidator(dateTimeProvider))
            .WithMessage($"'{nameof(ProcessPaymentRequest.CardExpiryDate)}' can't be in the past");

        RuleFor(request => request.Amount)
            .NotNull()
            .GreaterThan(0.0m)
            .WithMessage($"'{nameof(ProcessPaymentRequest.Amount)}' has to be positive");

        RuleFor(request => request.CurrencyCode)
            .NotNull()
            .Length(3);

        RuleFor(request => request.CardCvv)
            .NotNull()
            .Length(3)
            .WithMessage($"'{nameof(ProcessPaymentRequest.CardCvv)}' has to be 3 digits");
    }
}
