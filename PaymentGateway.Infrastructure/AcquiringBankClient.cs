using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.ProcessPayment;
using PaymentGateway.Domain;
using System.Net;
using System.Text.Json;

namespace PaymentGateway.Infrastructure;

public class AcquiringBankClient : IAcquiringBankClient
{
    private readonly Dictionary<Guid, PaymentStatus> _paymentIdToStatus = new();

    public Task<HttpResponseMessage> ProcessPaymentAsync(string data)
    {
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        var paymentId = Guid.NewGuid();
        _paymentIdToStatus.Add(paymentId, PaymentStatus.Pending);

        var content = new ProcessPaymentSuccessfulResponse(paymentId, _paymentIdToStatus[paymentId]);
        response.Content = new StringContent(JsonSerializer.Serialize(content));

        return Task.FromResult(response);
    }

    public Task<HttpResponseMessage> GetPaymentStatusAsync(string data)
    {
        var request = JsonSerializer.Deserialize<GetPaymentDetailsRequest>(data);
        if (request == null)
        {
            throw new JsonException();
        }

        var response = new HttpResponseMessage();
        var content = new GetPaymentStatusResponse(_paymentIdToStatus[request.PaymentId]);
        response.Content = new StringContent(JsonSerializer.Serialize(content));
        return Task.FromResult(response);
    }
}
