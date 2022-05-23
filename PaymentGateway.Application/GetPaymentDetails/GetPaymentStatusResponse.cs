using PaymentGateway.Domain;

namespace PaymentGateway.Application.GetPaymentDetails;

public record class GetPaymentStatusResponse(PaymentStatus Status);