using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.ValueObjects;
using System.Text.Json;

namespace PaymentGateway.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<PaymentDetails> PaymentDetails => Set<PaymentDetails>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Card>()
            .HasKey(card => new { card.Number, card.ExpiryDate, card.Cvv });

        modelBuilder
            .Entity<PaymentDetails>()
            .HasKey(paymentDetails => paymentDetails.Id);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        modelBuilder
            .Entity<Card>()
            .Property(card => card.ExpiryDate)
            .HasConversion(
                from => JsonSerializer.Serialize(from, (JsonSerializerOptions)null),
                to => JsonSerializer.Deserialize<CardExpiryDate>(to, (JsonSerializerOptions)null));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}