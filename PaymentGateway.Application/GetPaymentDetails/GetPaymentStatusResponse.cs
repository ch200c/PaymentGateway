using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.GetPaymentDetails;

public record class GetPaymentStatusResponse(PaymentStatus Status);