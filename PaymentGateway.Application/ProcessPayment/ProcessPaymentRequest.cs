using PaymentGateway.Domain;

namespace PaymentGateway.Application.ProcessPayment;

public record class ProcessPaymentRequest(
    string CardNumber, CardExpiryDate ExpiryDate, string Cvv, decimal Amount, string CurrencyCode);