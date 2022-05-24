using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.ProcessPayment;
using PaymentGateway.Domain;

namespace PaymentGateway.IntegrationTests;

public class ProcessPaymentTests
{
    [Fact]
    public async Task ProcessPayment_ValidRequest_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var dateTime = new DateTime(2022, 5, 24);
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        dateTimeProviderMock
            .Setup(dateTimeProvider => dateTimeProvider.GetDateTime())
            .Returns(dateTime);

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveService(typeof(IDateTimeProvider));
                    services.AddTransient(_ => dateTimeProviderMock.Object);
                });
            });

        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2023, 3), "123", 10.0m, "EUR");
        var client = application.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/Payments", request);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ProcessPayment_InvalidRequest_ShouldReturnUnsuccessfulResponse()
    {
        // Arrange
        var dateTime = new DateTime(2022, 5, 24);
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        dateTimeProviderMock
            .Setup(dateTimeProvider => dateTimeProvider.GetDateTime())
            .Returns(dateTime);

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveService(typeof(IDateTimeProvider));
                    services.AddTransient(_ => dateTimeProviderMock.Object);
                });
            });

        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2020, 3), "123", 10.0m, "EUR");
        var client = application.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/Payments", request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
}