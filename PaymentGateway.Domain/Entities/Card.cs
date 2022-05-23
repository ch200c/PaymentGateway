namespace PaymentGateway.Domain.Entities;

public class Card
{
    public string Number { get; set; } = null!;
    public CardExpiryDate ExpiryDate { get; set; }
    public string Cvv { get; set; } = null!;
}