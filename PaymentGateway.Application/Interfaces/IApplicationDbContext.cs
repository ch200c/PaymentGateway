using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Card> Cards { get; }
    DbSet<PaymentDetails> PaymentDetails { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}