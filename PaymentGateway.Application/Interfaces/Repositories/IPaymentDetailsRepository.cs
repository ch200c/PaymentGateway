using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.Interfaces.Repositories;

public interface IPaymentDetailsRepository
{
    public Task<PaymentDetails> InsertPaymentDetailsAsync(
        Guid id, decimal amount, Card card, string currencyCode, PaymentStatus status);
    public Task<PaymentDetails?> GetPaymentDetailsAsync(Guid id);
}