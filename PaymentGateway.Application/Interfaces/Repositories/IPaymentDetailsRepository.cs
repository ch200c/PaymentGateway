using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.Interfaces.Repositories;

public interface IPaymentDetailsRepository
{
    public Task<PaymentDetails> InsertAsync(Guid id, decimal amount, Card card, string currencyCode, PaymentStatus status);
    public Task<PaymentDetails?> GetAsync(Guid id);
}