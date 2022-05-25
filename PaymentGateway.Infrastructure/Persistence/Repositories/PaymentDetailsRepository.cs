using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Interfaces.Repositories;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Infrastructure.Persistence.Repositories;

public class PaymentDetailsRepository : IPaymentDetailsRepository
{
    private readonly IApplicationDbContext _dbContext;

    public PaymentDetailsRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PaymentDetails?> GetAsync(Guid id)
    {
        return _dbContext.PaymentDetails
            .Include(paymentDetails => paymentDetails.Card)
            .SingleOrDefaultAsync(paymentDetails => paymentDetails.Id == id);
    }

    public async Task<PaymentDetails> InsertAsync(
        Guid id, decimal amount, Card card, string currencyCode, PaymentStatus status)
    {
        var paymentDetails = new PaymentDetails()
        {
            Id = id,
            Amount = amount,
            Card = card,
            CurrencyCode = currencyCode,
            Status = status
        };

        await _dbContext.PaymentDetails.AddAsync(paymentDetails);
        await _dbContext.SaveChangesAsync();

        return paymentDetails;
    }
}
