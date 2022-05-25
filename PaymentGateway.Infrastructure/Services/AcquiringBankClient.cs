using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.ProcessPayment;
using PaymentGateway.Domain.Enums;
using System.Net;
using System.Text.Json;

namespace PaymentGateway.Infrastructure.Services;

public class AcquiringBankClient : IAcquiringBankClient
{
    private readonly Dictionary<Guid, PaymentStatus> _paymentIdToStatus = new();

    public Task<HttpResponseMessage> ProcessPaymentAsync(string data)
    {
        var paymentId = Guid.NewGuid();
        _paymentIdToStatus.Add(paymentId, PaymentStatus.Pending);

        var content = new ProcessPaymentResponse(paymentId, _paymentIdToStatus[paymentId]);

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(content))
        };

        return Task.FromResult(response);
    }

    public Task<HttpResponseMessage> GetPaymentStatusAsync(string data)
    {
        var request = JsonSerializer.Deserialize<GetPaymentDetailsRequest>(data);
        if (request == null)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        var content = new GetPaymentStatusResponse(_paymentIdToStatus[request.PaymentId]);

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(content))
        };

        return Task.FromResult(response);
    }
}
