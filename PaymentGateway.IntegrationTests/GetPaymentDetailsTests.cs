namespace PaymentGateway.IntegrationTests;

[Collection("Sequential")]
public class GetPaymentDetailsTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly WebApplicationFactory<Program> _applicationFactory;

    public GetPaymentDetailsTests()
    {
        _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

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
    public async Task GetPaymentDetails_ExistingPayment_ShouldReturnPaymentDetails()
    {
        // Arrange
        using var scope = _applicationFactory.Services.CreateScope();
        var services = scope.ServiceProvider;

        var paymentService = services.GetRequiredService<IPaymentService>();

        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2023, 3), "123", 10.0m, "EUR");
        var paymentId = (await paymentService.ProcessPaymentAsync(request)).PaymentId;

        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/v1/Payments/{paymentId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var paymentDetails = JsonSerializer.Deserialize<GetPaymentDetailsResponse>(responseContent, _jsonSerializerOptions);

        // Assert
        Assert.NotNull(paymentDetails);
        Assert.NotEqual(request.CardNumber, paymentDetails!.CardNumber);
        Assert.Equal(request.CardExpiryDate, paymentDetails.CardExpiryDate);
        Assert.Equal(request.CardCvv, paymentDetails.CardCvv);
        Assert.Equal(PaymentStatus.Pending, paymentDetails.Status);
    }
}