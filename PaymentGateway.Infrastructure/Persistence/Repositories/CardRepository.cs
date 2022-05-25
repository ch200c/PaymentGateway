using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Interfaces.Repositories;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.ValueObjects;

namespace PaymentGateway.Infrastructure.Persistence.Repositories;

public class CardRepository : ICardRepository
{
    private readonly IApplicationDbContext _dbContext;

    public CardRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Card> GetOrInsertAsync(string number, CardExpiryDate expiryDate, string cvv)
    {
        var card = _dbContext.Cards
            .SingleOrDefault(card => card.Number == number && card.ExpiryDate == expiryDate && card.Cvv == cvv);

        if (card != null)
        {
            return card;
        }

        card = new Card()
        {
            Number = number,
            ExpiryDate = expiryDate,
            Cvv = cvv
        };

        await _dbContext.Cards.AddAsync(card);
        await _dbContext.SaveChangesAsync();

        return card;
    }
}
