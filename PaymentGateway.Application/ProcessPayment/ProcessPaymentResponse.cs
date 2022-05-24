using PaymentGateway.Domain;

namespace PaymentGateway.Application.ProcessPayment;

public record class ProcessPaymentResponse(Guid PaymentId, PaymentStatus Status);