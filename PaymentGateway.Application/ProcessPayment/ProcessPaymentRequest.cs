using PaymentGateway.Domain.ValueObjects;

namespace PaymentGateway.Application.ProcessPayment;

public record class ProcessPaymentRequest(
    string CardNumber, CardExpiryDate CardExpiryDate, string CardCvv, decimal Amount, string CurrencyCode);