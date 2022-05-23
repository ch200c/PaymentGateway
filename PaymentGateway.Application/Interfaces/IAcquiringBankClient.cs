namespace PaymentGateway.Application.Interfaces;

public interface IAcquiringBankClient
{
    Task<HttpResponseMessage> ProcessPaymentAsync(string data);
    Task<HttpResponseMessage> GetPaymentStatusAsync(string data);
}
