namespace PaymentGateway.UnitTests;

public class ExpiryDateValidatorTests
{
    private readonly ExpiryDateValidator _validator;

    public ExpiryDateValidatorTests()
    {
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        dateTimeProviderMock
           .Setup(dateTimeProvider => dateTimeProvider.GetDateTime())
           .Returns(new DateTime(2022, 5, 25));

        _validator = new ExpiryDateValidator(dateTimeProviderMock.Object);
    }

    [Fact]
    public void Validate_EmptyCardExpiryDate_ShouldFailValidation()
    {
        // Arrange
        var expiryDate = new CardExpiryDate();

        // Act
        var result = _validator.Validate(expiryDate);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_SameYearLaterMonth_ShouldPassValidation()
    {
        // Arrange
        var expiryDate = new CardExpiryDate(2022, 6);

        // Act
        var result = _validator.Validate(expiryDate);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_LaterYearEarlierMonth_ShouldPassValidation()
    {
        // Arrange
        var expiryDate = new CardExpiryDate(2023, 1);

        // Act
        var result = _validator.Validate(expiryDate);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_LaterDate_ShouldPassValidation()
    {
        // Arrange
        var expiryDate = new CardExpiryDate(2023, 12);

        // Act
        var result = _validator.Validate(expiryDate);

        // Assert
        Assert.True(result.IsValid);
    }
}
