namespace PaymentGateway.IntegrationTests;

[Collection("Sequential")]
public class ProcessPaymentTests
{
    private readonly WebApplicationFactory<Program> _applicationFactory;

    public ProcessPaymentTests()
    {
        _applicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddMockDateTimeProvider(new DateTime(2022, 5, 24));
                });
            });
    }

    [Fact]
    public async Task ProcessPayment_ValidRequest_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2023, 3), "123", 10.0m, "EUR");
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Payments", request);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ProcessPayment_InvalidRequest_ShouldReturnUnsuccessfulResponse()
    {
        // Arrange
        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2020, 3), "123", 10.0m, "EUR");
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/Payments", request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
}