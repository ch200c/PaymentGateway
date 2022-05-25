using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.ValueObjects;

namespace PaymentGateway.Application.Interfaces.Repositories;

public interface ICardRepository
{
    public Task<Card> GetOrInsertAsync(string number, CardExpiryDate expiryDate, string cvv);
}