using PaymentGateway.Domain;

namespace PaymentGateway.Application.ProcessPayment;

public record class ProcessPaymentSuccessfulResponse(Guid PaymentId, PaymentStatus Status);