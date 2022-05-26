using PaymentGateway.Application.ProcessPayment;

namespace PaymentGateway.UnitTests;

public class ProcessPaymentRequestValidatorTests
{
    private readonly ProcessPaymentRequestValidator _validator;

    public ProcessPaymentRequestValidatorTests()
    {
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        dateTimeProviderMock
           .Setup(dateTimeProvider => dateTimeProvider.GetDateTime())
           .Returns(new DateTime(2022, 5, 25));

        _validator = new ProcessPaymentRequestValidator(dateTimeProviderMock.Object);
    }

    [Fact]
    public void Validate_ValidRequest_ShouldPassValidation()
    {
        // Arrange
        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2023, 3), "123", 10.0m, "EUR");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidLengthCardNumber_ShouldFailValidation()
    {
        // Arrange
        var request = new ProcessPaymentRequest("123456789012345", new CardExpiryDate(2023, 3), "123", 10.0m, "EUR");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_NegativeAmount_ShouldFailValidation()
    {
        // Arrange
        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2023, 3), "123", -10.0m, "EUR");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidLengthCardCvv_ShouldFailValidation()
    {
        // Arrange
        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2023, 3), "12", 10.0m, "EUR");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidLengthCurrencyCode_ShouldFailValidation()
    {
        // Arrange
        var request = new ProcessPaymentRequest("1234567890123456", new CardExpiryDate(2023, 3), "123", 10.0m, "EU");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }
}
