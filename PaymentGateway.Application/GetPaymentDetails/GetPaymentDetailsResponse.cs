using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.ValueObjects;

namespace PaymentGateway.Application.GetPaymentDetails;

public record class GetPaymentDetailsResponse(
    string CardNumber, CardExpiryDate CardExpiryDate, string CardCvv, decimal Amount, string CurrencyCode, PaymentStatus Status);