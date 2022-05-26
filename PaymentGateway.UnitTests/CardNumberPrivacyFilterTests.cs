namespace PaymentGateway.UnitTests;

public class CardNumberPrivacyFilterTests
{
    [Fact]
    public void Mask_Input_ShouldReturnMaskedInput()
    {
        // Arrange
        var input = "12345678";
        var filter = new CardNumberPrivacyFilter('X');

        // Act
        var output = filter.Mask(input, 4);

        // Assert
        Assert.Equal("XXXX5678", output);
    }
}