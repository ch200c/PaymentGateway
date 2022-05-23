using OneOf;
using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.ProcessPayment;

namespace PaymentGateway.Application.Interfaces;

public interface IPaymentService
{
    Task<OneOf<
        ProcessPaymentSuccessfulResponse,
        ProcessPaymentUnsuccessfulResponse>> ProcessPaymentAsync(ProcessPaymentRequest request);

    Task<GetPaymentDetailsResponse?> GetPaymentDetailsAsync(GetPaymentDetailsRequest request);
}