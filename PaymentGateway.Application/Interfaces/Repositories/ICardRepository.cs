using PaymentGateway.Domain;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Interfaces.Repositories;

public interface ICardRepository
{
    public Task<Card> UpsertCardAsync(string number, CardExpiryDate expiryDate, string cvv);
}