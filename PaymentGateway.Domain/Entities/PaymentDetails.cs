using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Entities;

public class PaymentDetails
{
    public Guid Id { get; set; }
    public Card Card { get; set; } = null!;
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public PaymentStatus Status { get; set; }
}