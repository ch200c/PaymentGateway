using PaymentGateway.Application.Interfaces;

namespace PaymentGateway.Api.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetDateTime()
    {
        return DateTime.UtcNow;
    }
}