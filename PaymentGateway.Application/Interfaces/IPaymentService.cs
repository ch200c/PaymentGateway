using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.ProcessPayment;

namespace PaymentGateway.Application.Interfaces;

public interface IPaymentService
{
    Task<ProcessPaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request);

    Task<GetPaymentDetailsResponse?> GetPaymentDetailsAsync(GetPaymentDetailsRequest request);
}