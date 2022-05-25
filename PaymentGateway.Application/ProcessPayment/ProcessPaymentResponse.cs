using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.ProcessPayment;

public record class ProcessPaymentResponse(Guid PaymentId, PaymentStatus Status);