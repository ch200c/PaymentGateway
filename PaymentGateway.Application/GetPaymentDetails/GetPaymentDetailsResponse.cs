using PaymentGateway.Domain;

namespace PaymentGateway.Application.GetPaymentDetails;

public record class GetPaymentDetailsResponse(
    string CardNumber, CardExpiryDate ExpiryDate, string Cvv, decimal Amount, string CurrencyCode, PaymentStatus Status);